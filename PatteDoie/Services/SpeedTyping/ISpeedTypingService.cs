using PatteDoie.Models.Platform;
using PatteDoie.Models.SpeedTyping;
using PatteDoie.Queries.SpeedTyping;
using PatteDoie.Rows.SpeedTyping;
using PatteDoie.Rows.SpeedTypingGame;

namespace PatteDoie.Services.SpeedTyping
{
    public interface ISpeedTypingService
    {
        public Task<SpeedTypingGameRow> GetGame(Guid gameId);

        public Task SetTimeProgress(SpeedTypingGame game, SpeedTypingPlayer player, DateTime timeProgress);

        public Task<SpeedTypingGameRow> CreateGame(Lobby lobby);

        public Task DeleteGame(Guid id);

        public Task<bool> CheckWord(Guid gameId, Guid playerId, string word);

        public Task<int> GetScore(Guid playerId);
        public Task ManageEndOfGame(Guid gameId);

        public Task<bool> CanPlay(Guid playerId, Guid gameId);
        public Task<List<SpeedTypingPlayerRow>> GetRank(Guid gameId);
    }
}
