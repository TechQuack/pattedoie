using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PatteDoie.Models.Platform;
using PatteDoie.PatteDoieException;
using PatteDoie.Queries.Platform;
using PatteDoie.Rows.Platform;

namespace PatteDoie.Services.Platform
{
    public class PlatformService(PatteDoieContext context, IMapper mapper) : IPlatformService
    {
        private readonly PatteDoieContext _context = context;
        private readonly IMapper _mapper = mapper;


        public async Task<PlatformLobbyRow> CreateLobby(Guid creatorId, string creatorName, string? password)
        {
            var creator = new PlatformUser
            {
                Id = creatorId,
                Nickname = creatorName
            };

            var platformLobby = new PlatformLobby
            {
                Creator = creator,
                Password = null,
                Users = new List<PlatformUser>()
            };

            if (!password.IsNullOrEmpty())
            {
                PasswordHasher<PlatformLobby> passwordHasher = new();

                platformLobby.Password = passwordHasher.HashPassword(platformLobby, password);
            }

            _context.PlatformLobby.Add(platformLobby);
            _context.PlatformUser.Add(creator);

            await _context.SaveChangesAsync();

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
            var lobby = await _context.PlatformLobby.AsQueryable().Include(l => l.Creator).Where(l => l.Id == lobbyId).FirstOrDefaultAsync() ?? throw new LobbyNotFoundException("Lobby not found");
            return _mapper.Map<PlatformLobbyRow>(lobby);
        }

        public Task<IEnumerable<PlatformLobbyRow>> SearchLobbies(CreatePlatformLobbyCommand command)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLobby(Guid lobbyId, CreatePlatformLobbyCommand command)
        {
            throw new NotImplementedException();
        }

        public async Task<PlatformUserRow> JoinLobby(Guid lobbyId, string nickname, Guid userUUID, string? password)
        {

            var lobby = await _context.PlatformLobby.AsQueryable().Where(l => l.Id == lobbyId).FirstOrDefaultAsync() ?? throw new LobbyNotFoundException("Lobby not found");

            if (password.IsNullOrEmpty())
            {
                if (!lobby.Password.IsNullOrEmpty())
                {
                    throw new PasswordNotValidException("Lobby password is not valid");
                }
            }
            else
            {
                PasswordHasher<PlatformLobby> passwordHasher = new();
                var result = passwordHasher.VerifyHashedPassword(lobby, lobby.Password, password);
                if (result != PasswordVerificationResult.Success)
                {
                    throw new PasswordNotValidException("Password is not valid");
                }
            }

            var platformUser = new PlatformUser
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

    }
}
