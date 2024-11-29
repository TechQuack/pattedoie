using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PatteDoie.Enums;
using PatteDoie.Extensions;
using PatteDoie.Models.Platform;
using PatteDoie.PatteDoieException;
using PatteDoie.Queries.Platform;
using PatteDoie.Rows.Platform;
using PatteDoie.Services.Scattergories;
using PatteDoie.Services.SpeedTyping;

namespace PatteDoie.Services.Platform
{
    public class PlatformService(PatteDoieContext context, IMapper mapper, ISpeedTypingService speedTypingService, IScattegoriesService scattergoriesService) : IPlatformService
    {
        private readonly PatteDoieContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ISpeedTypingService _speedTypingService = speedTypingService;
        private readonly IScattegoriesService _scattergoriesService = scattergoriesService;


        public async Task<PlatformLobbyRow> CreateLobby(Guid creatorId, string creatorName, string? password, GameType type)
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
            switch(gameType)
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

            return _mapper.Map<List<PlatformLobbyRow>>(await query.Include(l => l.Game).ToListAsync());
        }

        public Task UpdateLobby(Guid lobbyId, CreatePlatformLobbyCommand command)
        {
            throw new NotImplementedException();
        }

        public async Task<PlatformUserRow> JoinLobby(Guid lobbyId, string nickname, Guid userUUID, string? password)
        {

            var lobby = await _context.PlatformLobby.AsQueryable().Where(l => l.Id == lobbyId).FirstOrDefaultAsync() ?? throw new LobbyNotFoundException("Lobby not found");

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

            return _mapper.Map<PlatformUserRow>(platformUser);
        }

        public async Task<PlatformUserRow> GetUser(Guid userId)
        {
            var creator = await _context.PlatformUser.AsQueryable().Where(u => u.UserUUID == userId).FirstOrDefaultAsync();
            return _mapper.Map<PlatformUserRow>(creator);
        }

        public async Task<IEnumerable<PlatformHighScoreRow>> GetHighestScoreFromGame(Guid gameId)
        {
            var highScores = await _context.PlatformHighScore.AsQueryable().Where(g => g.Id == gameId).OrderDescending().Take(5).ToListAsync() ??
                throw new HighScoreNotFoundException("HighScores not found");
            return _mapper.Map<List<PlatformHighScoreRow>>(highScores);
        }

        public async Task<Guid?> StartGame(Guid lobbyId)
        {
            var lobby = await _context.PlatformLobby.AsQueryable()
                .Include(l => l.Game)
                .FirstOrDefaultAsync(l => l.Id == lobbyId) ?? throw new LobbyNotFoundException("Lobby not found");
            lobby.Started = true;
            _context.PlatformLobby.Update(lobby);
            await _context.SaveChangesAsync();
            var id = await CreateGame(GameTypeHelper.GetGameTypeFromString(lobby.Game.Name), lobby);
            return id;
        }

        private async Task<Guid?> CreateGame(GameType type, Lobby lobby)
        {
            var users = lobby.Users;
            switch (type)
            {
                case GameType.Scattergories:
                    // Verify number of users
                    var numCat = 5;
                    var numRound = 5;
                    var gameScattergories = await _scattergoriesService.CreateGame(numCat, numRound, users, lobby.Creator);
                    return gameScattergories.Id;
                case GameType.SpeedTyping:
                    // Verify number of users
                    var gameSpeedTyping = await _speedTypingService.CreateGame(users);
                    return gameSpeedTyping.Id;
            }
            return null;
        }
    }
}
