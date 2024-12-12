using PatteDoie.Enums;
using PatteDoie.Queries.Platform;
using PatteDoie.Rows.Platform;

namespace PatteDoie.Services.Platform
{
    public interface IPlatformService
    {

        public Task<IEnumerable<PlatformLobbyRow>> GetAllLobbies();

        public Task<PlatformLobbyRow> GetLobby(Guid lobbyId);

        public Task<IEnumerable<PlatformLobbyRow>> SearchLobbies(LobbyType type, FilterGameType gameType);

        public Task<PlatformLobbyRow> CreateLobby(Guid creatorId, string creatorName, string? password, GameType type, string lobbyName);

        public Task<PlatformUserRow> JoinLobby(Guid lobbyId, string nickname, Guid userUUID, string? password);

        public Task<PlatformUserRow> GetUser(Guid userId, Guid lobbyId);

        public Task<IEnumerable<PlatformHighScoreRow>> GetHighestScoreFromGame(Guid gameId);

        public Task<Guid?> StartGame(Guid lobbyId, Guid playerId);

        public Task<Guid?> GetGameUUIDFromLobby(Guid lobbyId);

        public Task<List<PlatformHighScoreRow>> GetGameHighScores(string gameName);

        public Task<Boolean> IsHost(Guid playerId, Guid creatorId, Guid lobbyId);
        public Task<Boolean> IsInLobby(Guid playerId, Guid lobbyId);
    }
}
