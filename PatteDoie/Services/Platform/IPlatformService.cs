using PatteDoie.Enums;
using PatteDoie.Queries.Platform;
using PatteDoie.Rows.Platform;

namespace PatteDoie.Services.Platform
{
    public interface IPlatformService
    {

        public Task<IEnumerable<PlatformLobbyRow>> GetAllLobbies();

        public Task<PlatformLobbyRow> GetLobby(Guid lobbyId);

        public Task<IEnumerable<PlatformLobbyRow>> GetLobbiesByGame(Guid gameId);

        public Task<IEnumerable<PlatformLobbyRow>> SearchLobbies(LobbyType type);

        public Task UpdateLobby(Guid lobbyId, CreatePlatformLobbyCommand command);

        public Task<PlatformLobbyRow> CreateLobby(Guid creatorId, string creatorName, string? password, GameType type);

        public Task<PlatformUserRow> JoinLobby(Guid lobbyId, string nickname, Guid userUUID, string? password);

        public Task<PlatformUserRow> GetUser(Guid userId);

        public Task<IEnumerable<PlatformHighScoreRow>> GetHighestScoreFromGame(Guid gameId);

        public Task<bool> StartGame(Guid lobbyId);
    }
}
