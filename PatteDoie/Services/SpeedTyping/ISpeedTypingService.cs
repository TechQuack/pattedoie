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

        public Task<SpeedTypingGameRow> CreateGame(CreateSpeedTypingGameCommand command, PlatformUser[] platformUsers);

        public Task DeleteGame(Guid id);

        public Task<bool> CheckWord(Guid gameId, Guid playerId, string word);
    }
}
