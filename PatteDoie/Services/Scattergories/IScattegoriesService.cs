using PatteDoie.Models.Platform;
using PatteDoie.Models.Scattergories;
using PatteDoie.Rows.Scattegories;

namespace PatteDoie.Services.Scattergories
{
    public interface IScattegoriesService
    {
        public Task<IEnumerable<ScattergoriesGameRow>> GetAllGames();
        public Task<ScattergoriesGameRow> GetGame(Guid gameId);
        public Task<IEnumerable<ScattergoriesGameRow>> SearchGames(/*TODO*/);
        public Task UpdateGame(Guid gameId /*, TODO*/);
        public Task<ScattergoriesGameRow> AddPlayerWord(ScattergoriesGame game, ScattergoriesPlayer player, string word, ScattergoriesCategory category);
        public Task<ScattergoriesGameRow> NextRound(ScattergoriesGame game);
        public Task<ScattergoriesGameRow> CreateGame(int numberCategories, int roundNumber, Lobby lobby);
        public Task DeleteGame(Guid gameId);
        public Task<ScattergoriesGameRow> EndScattergoriesGame(ScattergoriesGame game);
        public Task<ScattergoriesGameRow> HostVerifyWord(ScattergoriesGame game, ScattergoriesPlayer player, ScattergoriesAnswer answer, bool decision);
    }
}
