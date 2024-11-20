using AutoMapper;
using PatteDoie.Models.Platform;
using PatteDoie.Queries.Platform;
using PatteDoie.Rows.Platform;

namespace PatteDoie.Services.Platform
{
    public class PlatformService : IPlatformService
    {

        private readonly PatteDoieContext _context;

        private readonly IMapper _mapper;

        public PlatformService(PatteDoieContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PlatformLobbyRow> CreateLobby(CreatePlatformLobbyCommand command, PlatformUser creator, string password)
        {

            var PlatformLobby = new PlatformLobby
            {
                users = [creator],
                creator = creator,
                password = password,
                started = false
            };

            _context.PlatformLobby.Add(PlatformLobby);

            await _context.SaveChangesAsync();

            return _mapper.Map<PlatformLobbyRow>(PlatformLobby);
        }

        public Task<IEnumerable<PlatformLobbyRow>> GetAllLobbies()
        {
            throw new NotImplementedException();
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
    }
}
