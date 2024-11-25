using PatteDoie.Models.Platform;
using PatteDoie.Models.SpeedTyping;
using PatteDoie.Queries.SpeedTyping;
using PatteDoie.Rows.SpeedTypingGame;

namespace PatteDoie.Services.SpeedTyping
{
    public interface ISpeedTypingService
    {
        public Task<IEnumerable<SpeedTypingGameRow>> GetAllGames();

        public Task<SpeedTypingGameRow> GetGame(Guid gameId);

        public Task<IEnumerable<SpeedTypingGameRow>> SearchGames(CreateSpeedTypingGameCommand query);

        public Task SetTimeProgress(SpeedTypingGame game, SpeedTypingPlayer player, DateTime timeProgress);

        public Task<SpeedTypingGameRow> CreateGame(List<User> platformUsers);
            
        public Task DeleteGame(Guid id);

        public Task<bool> CheckWord(Guid gameId, Guid playerId, string word);
    }
}
