using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PatteDoie.Enums;
using PatteDoie.Extensions;
using PatteDoie.Models.Platform;
using PatteDoie.PatteDoieException;
using PatteDoie.Queries.Platform;
using PatteDoie.Rows.Platform;
using PatteDoie.Services.SpeedTyping;

namespace PatteDoie.Services.Platform
{
    public class PlatformService(PatteDoieContext context, IMapper mapper, ISpeedTypingService speedTypingService) : IPlatformService
    {
        private readonly PatteDoieContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ISpeedTypingService _speedTypingService = speedTypingService;


        public async Task<PlatformLobbyRow> CreateLobby(Guid creatorId, string creatorName, string? password, GameType type)
        {
            var gameName = type.GetDescription();
            var game = await _context.PlatformGame.AsQueryable().Where(g => g.Name == gameName).FirstOrDefaultAsync() ?? throw new GameNotFoundException("Game not found");

            var creator = new User
            {
                Id = creatorId,
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
            var lobby = await _context.PlatformLobby.AsQueryable().Include(l => l.Creator).Include(l => l.Game).Where(l => l.Id == lobbyId).FirstOrDefaultAsync() ?? throw new LobbyNotFoundException("Lobby not found");
            return _mapper.Map<PlatformLobbyRow>(lobby);
        }

        public async Task<IEnumerable<PlatformLobbyRow>> SearchLobbies(LobbyType type)
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
            return _mapper.Map<List<PlatformLobbyRow>>(await query.ToListAsync());
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

        public async Task<bool> StartGame(Guid lobbyId)
        {
            var lobby = await _context.PlatformLobby.AsQueryable().Where(l => l.Id == lobbyId).FirstOrDefaultAsync() ?? throw new LobbyNotFoundException("Lobby not found");
            lobby.Started = true;
            _context.PlatformLobby.Update(lobby);
            await _context.SaveChangesAsync();
            await CreateGame(GameTypeHelper.GetGameTypeFromString(lobby.Game.Name), lobby.Users);
            return true;
        }

        private async Task CreateGame(GameType type, List<User> users)
        {
            switch(type)
            {
                case GameType.Scattergories:
                    //TODO : Create game
                    break;
                case GameType.SpeedTyping:
                    // Verify number of users
                    await _speedTypingService.CreateGame(users);
                    break;
            }
        }
    }
}
