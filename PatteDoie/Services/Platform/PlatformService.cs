using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.Platform;
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

            if (password != null)
            {
                PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

                password = passwordHasher.HashPassword(password, password);
            }


            var creator = new PlatformUser
            {
                Id = creatorId,
                Nickname = creatorName
            };

            var PlatformLobby = new PlatformLobby
            {
                Creator = creator,
                Password = password
            };

            _context.PlatformLobby.Add(PlatformLobby);
            _context.PlatformUser.Add(creator);

            await _context.SaveChangesAsync();

            return _mapper.Map<PlatformLobbyRow>(PlatformLobby);
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

        public Task<IEnumerable<PlatformLobbyRow>> GetLobby(Guid lobbyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PlatformLobbyRow>> SearchLobbies(CreatePlatformLobbyCommand command)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLobby(Guid lobbyId, CreatePlatformLobbyCommand command)
        {
            throw new NotImplementedException();
        }

        async Task<PlatformUserRow> IPlatformService.CreateUser(CreatePlatformUserCommand command, string nickname)
        {
            var PlatformUser = new PlatformUser
            {
                Nickname = nickname,
            };

            _context.PlatformUser.Add(PlatformUser);

            await _context.SaveChangesAsync();

            return _mapper.Map<PlatformUserRow>(PlatformUser);
        }

        public async Task<PlatformUserRow> GetUser(Guid userId)
        {
            var creator = (await _context.PlatformUser.AsQueryable().Where(u => u.Id == userId).ToListAsync())[0];
            return _mapper.Map<PlatformUserRow>(creator);
        }

    }
}
