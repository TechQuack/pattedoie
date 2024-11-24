using PatteDoie.Models.Platform;
using PatteDoie.Queries.SpeedTyping;
using PatteDoie.Rows.SpeedTypingGame;

namespace PatteDoie.Services.SpeedTyping
{
    public interface ISpeedTypingService
    {
        public Task<IEnumerable<SpeedTypingGameRow>> GetAllGames();

        public Task<SpeedTypingGameRow> GetGame(Guid gameId);

        public Task<IEnumerable<SpeedTypingGameRow>> SearchGames(CreateSpeedTypingGameCommand query);

        public Task UpdateGame(Guid id, CreateSpeedTypingGameCommand game);

        public Task<SpeedTypingGameRow> CreateGame(CreateSpeedTypingGameCommand command, List<User> platformUsers);
            
        public Task DeleteGame(Guid id);
    }
}
