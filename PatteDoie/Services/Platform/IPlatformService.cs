using PatteDoie.Queries.Platform;
using PatteDoie.Rows.Platform;

namespace PatteDoie.Services.Platform
{
    public interface IPlatformService
    {

        public Task<IEnumerable<PlatformLobbyRow>> GetAllLobbies();

        public Task<PlatformLobbyRow> GetLobby(Guid lobbyId);

        public Task<IEnumerable<PlatformLobbyRow>> GetLobbiesByGame(Guid gameId);

        public Task<IEnumerable<PlatformLobbyRow>> GetPublicLobbies();

        public Task<IEnumerable<PlatformLobbyRow>> SearchLobbies(CreatePlatformLobbyCommand command);

        public Task UpdateLobby(Guid lobbyId, CreatePlatformLobbyCommand command);

        public Task<PlatformLobbyRow> CreateLobby(Guid creatorId, string creatorName, string? password);

        public Task<PlatformUserRow> JoinLobby(Guid lobbyId, string nickname, Guid userUUID, string? password);

        public Task<PlatformUserRow> GetUser(Guid userId);
    }
}
