using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PatteDoie.Enums;
using PatteDoie.Extensions;
using PatteDoie.Hubs;
using PatteDoie.Models.Platform;
using PatteDoie.PatteDoieException;
using PatteDoie.Queries.Platform;
using PatteDoie.Rows.Platform;
using PatteDoie.Services.Scattergories;
using PatteDoie.Services.SpeedTyping;

namespace PatteDoie.Services.Platform
{
    public class PlatformService(PatteDoieContext context,
        IMapper mapper,
        ISpeedTypingService speedTypingService,
        IScattergoriesService scattergoriesService,
        IHubContext<PlatformHub> hub) : IPlatformService
    {
        private readonly PatteDoieContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ISpeedTypingService _speedTypingService = speedTypingService;
        private readonly IScattergoriesService _scattergoriesService = scattergoriesService;
        private readonly IHubContext<PlatformHub> _hub = hub;


        public async Task<PlatformLobbyRow> CreateLobby(Guid creatorId, string creatorName, string? password, GameType type, string lobbyName)
        {
            var gameName = type.GetDescription();
            var game = await _context.PlatformGame.AsQueryable().Where(g => g.Name == gameName).FirstOrDefaultAsync() ?? throw new GameNotFoundException("Game not found");

            var creator = new User
            {
                UserUUID = creatorId,
                Nickname = creatorName
            };

            var platformLobby = new Lobby
            {
                Creator = creator,
                Password = null,
                Users = [],
                Game = game,
                LobbyName = lobbyName
            };

            if (!String.IsNullOrEmpty(password))
            {
                PasswordHasher<Lobby> passwordHasher = new();

                platformLobby.Password = passwordHasher.HashPassword(platformLobby, password);
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                await _context.PlatformLobby.AddAsync(platformLobby);
                await _context.PlatformUser.AddAsync(creator);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }

            platformLobby.Users.Add(creator);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                _context.PlatformLobby.Update(platformLobby);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }

            //TODO : Create game

            return _mapper.Map<PlatformLobbyRow>(platformLobby);
        }

        public async Task<IEnumerable<PlatformLobbyRow>> GetAllLobbies()
        {
            var lobbies = await _context.PlatformLobby.AsQueryable().ToListAsync();
            return _mapper.Map<List<PlatformLobbyRow>>(lobbies);
        }

        public Task<IEnumerable<PlatformLobbyRow>> GetLobbiesByGame(Guid gameId)
        {
            throw new NotImplementedException();
        }

        public async Task<PlatformLobbyRow> GetLobby(Guid lobbyId)
        {
            var lobby = await _context.PlatformLobby.AsQueryable()
                .Include(l => l.Creator)
                .Include(l => l.Game)
                .Include(l => l.Users)
                .FirstOrDefaultAsync(l => l.Id == lobbyId) ?? throw new LobbyNotFoundException("Lobby not found");
            return _mapper.Map<PlatformLobbyRow>(lobby);
        }

        public async Task<IEnumerable<PlatformLobbyRow>> SearchLobbies(LobbyType type, FilterGameType gameType)
        {
            var query = _context.PlatformLobby.AsQueryable();
            switch (type)
            {
                case LobbyType.Public:
                    query = query.Where(p => p.Password == null || p.Password == "");
                    break;
                case LobbyType.Private:
                    query = query.Where(p => p.Password != null && p.Password != "");
                    break;
                default:
                    break;
            }
            switch (gameType)
            {
                case FilterGameType.Scattergories:
                    query = query.Where(p => p.Game.Name == GameType.Scattergories.GetDescription());
                    break;
                case FilterGameType.SpeedTyping:
                    query = query.Where(p => p.Game.Name == GameType.SpeedTyping.GetDescription());
                    break;
                default:
                    break;
            }

            return _mapper.Map<List<PlatformLobbyRow>>(await query.Include(l => l.Game).Include(l => l.Users).ToListAsync());
        }

        public Task UpdateLobby(Guid lobbyId, CreatePlatformLobbyCommand command)
        {
            throw new NotImplementedException();
        }

        public async Task<PlatformUserRow> JoinLobby(Guid lobbyId, string nickname, Guid userUUID, string? password)
        {

            var lobby = await _context.PlatformLobby.AsQueryable().Where(l => l.Id == lobbyId).FirstOrDefaultAsync() ?? throw new LobbyNotFoundException("Lobby not found");

            if (IsLobbyContainingPlayer(userUUID, lobby))
            {
                throw new Exception("Lobby already contains this player");
            }

            if (String.IsNullOrEmpty(password))
            {
                if (!String.IsNullOrEmpty(lobby.Password))
                {
                    throw new PasswordNotValidException("Lobby password is not valid");
                }
            }
            else
            {
                PasswordHasher<Lobby> passwordHasher = new();
                var result = passwordHasher.VerifyHashedPassword(lobby, lobby.Password, password);
                if (result != PasswordVerificationResult.Success)
                {
                    throw new PasswordNotValidException("Password is not valid");
                }
            }

            var platformUser = new User
            {
                Nickname = nickname,
                UserUUID = userUUID
            };

            _context.PlatformUser.Add(platformUser);

            lobby.Users.Add(platformUser);

            if (lobby.Users.IsNullOrEmpty())
            {
                throw new Exception("User not added");
            }

            _context.PlatformLobby.Update(lobby);

            await _context.SaveChangesAsync();

            await _hub.Clients.Group(lobbyId.ToString())
                    .SendAsync("ReceivePlayerJoined", platformUser.UserUUID);

            return _mapper.Map<PlatformUserRow>(platformUser);
        }

        public async Task<PlatformUserRow> GetUser(Guid userId, Guid lobbyId)
        {
            var lobby = await _context.PlatformLobby.AsQueryable().Where(l => l.Id == lobbyId).FirstOrDefaultAsync();
            var user = lobby?.Users.Find(u => u.UserUUID == userId) ?? null;
            return _mapper.Map<PlatformUserRow>(user);
        }

        public async Task<IEnumerable<PlatformHighScoreRow>> GetHighestScoreFromGame(Guid gameId)
        {
            var highScores = await _context.PlatformHighScore.AsQueryable().Where(g => g.Id == gameId).OrderDescending().Take(5).ToListAsync() ??
                throw new HighScoreNotFoundException("HighScores not found");
            return _mapper.Map<List<PlatformHighScoreRow>>(highScores);
        }

        public async Task<Guid?> StartGame(Guid lobbyId, Guid playerId)
        {
            var lobby = await _context.PlatformLobby.AsQueryable()
                .Include(l => l.Game)
                .FirstOrDefaultAsync(l => l.Id == lobbyId) ?? throw new LobbyNotFoundException("Lobby not found");
            if (!await IsHost(playerId, lobby.Creator.UserUUID, lobby.Id))
            {
                throw new CreatorException("this player is not the host");
            }
            lobby.Started = true;
            _context.PlatformLobby.Update(lobby);
            await _context.SaveChangesAsync();
            var id = await CreateGame(GameTypeHelper.GetGameTypeFromString(lobby.Game.Name), lobby);
            if (id != null)
            {
                await _hub.Clients.Group(lobbyId.ToString())
                    .SendAsync("ReceiveGameStarted", id);
            }
            return id;
        }

        public async Task<Guid?> GetGameUUIDFromLobby(Guid lobbyId)
        {
            var lobby = await _context.PlatformLobby.AsQueryable()
                .Include(l => l.Game)
                .FirstOrDefaultAsync(l => l.Id == lobbyId) ?? throw new LobbyNotFoundException("Lobby not found");
            var gameType = GameTypeHelper.GetGameTypeFromString(lobby.Game.Name);
            switch (gameType)
            {
                case GameType.Scattergories:
                    return (await _context.ScattergoriesGame.AsQueryable().FirstOrDefaultAsync(g => g.Lobby.Id == lobbyId))?.Id;
                case GameType.SpeedTyping:
                    return (await _context.SpeedTypingGame.AsQueryable().FirstOrDefaultAsync(g => g.Lobby.Id == lobbyId))?.Id;
                default:
                    return null;
            }
        }

        public async Task<List<PlatformHighScoreRow>> GetGameHighScores(string gameName)
        {
            var platformGame = await _context.PlatformGame
                .Include(l => l.HighScores)
                .FirstOrDefaultAsync(p => p.Name == gameName)
                ?? throw new GameNotValidException("Game not valid");
            var highScores = platformGame.HighScores.OrderByDescending(l => l.Score).ToList();
            return _mapper.Map<List<PlatformHighScoreRow>>(highScores);
        }

        public async Task<Boolean> IsHost(Guid playerId, Guid creatorId, Guid lobbyId)
        {
            var player = await GetUser(playerId, lobbyId);
            return player != null && creatorId == player.UserUUID;
        }

        private async Task<Guid?> CreateGame(GameType type, Lobby lobby)
        {
            switch (type)
            {
                case GameType.Scattergories:
                    // Verify number of users
                    var numCat = 5;
                    var numRound = 5;
                    var gameScattergories = await _scattergoriesService.CreateGame(numCat, numRound, lobby);
                    return gameScattergories.Id;
                case GameType.SpeedTyping:
                    // Verify number of users
                    var gameSpeedTyping = await _speedTypingService.CreateGame(lobby);
                    return gameSpeedTyping.Id;
            }
            return null;
        }

        private bool IsLobbyContainingPlayer(Guid userUUID, Lobby lobby)
        {
            var players = lobby.Users;
            foreach (var player in players)
            {
                if (player.UserUUID == userUUID) return true;
            }
            return false;
        }
    }
}
