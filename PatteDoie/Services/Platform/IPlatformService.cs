using PatteDoie.Models.Platform;
using PatteDoie.Queries.Platform;
using PatteDoie.Rows.Platform;

namespace PatteDoie.Services.Platform
{
    public interface IPlatformService
    {

        public Task<IEnumerable<PlatformLobbyRow>> GetAllLobbies();

        public Task<IEnumerable<PlatformLobbyRow>> GetLobby(Guid lobbyId);

        public Task<IEnumerable<PlatformLobbyRow>> GetLobbiesByGame(Guid gameId);

        public Task<IEnumerable<PlatformLobbyRow>> SearchLobbies(CreatePlatformLobbyCommand command);

        public Task UpdateLobby(Guid lobbyId, CreatePlatformLobbyCommand command);

        public Task<PlatformLobbyRow> CreateLobby(CreatePlatformLobbyCommand command, PlatformUser creator, string password);

        public Task<PlatformUserRow> CreateUser(CreatePlatformUserCommand command, string nickname);
    }
}
