using PatteDoie.Models.Platform;
using PatteDoie.Queries.SpeedTyping;
using PatteDoie.Rows.SpeedTyping;
using PatteDoie.Rows.SpeedTypingGame;

namespace PatteDoie.Services.SpeedTyping
{
    public interface ISpeedTypingService
    {
        public Task<IEnumerable<SpeedTypingGameRow>> GetAllGames();

        public Task<SpeedTypingGameRow> GetGame(Guid gameId);

        public Task<IEnumerable<SpeedTypingGameRow>> SearchGames(CreateSpeedTypingGameCommand query);

        public Task<SpeedTypingGameRow> CreateGame(Lobby lobby);

        public Task DeleteGame(Guid id);

        public Task<bool> CheckWord(Guid gameId, Guid playerId, string word);

        public Task<int> GetScore(Guid playerId);
        public Task<bool> CanPlay(Guid playerId, Guid gameId);
        public Task<List<SpeedTypingPlayerRow>> GetRank(Guid gameId);
    }
}
