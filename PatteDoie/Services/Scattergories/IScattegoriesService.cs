using PatteDoie.Models.Platform;
using PatteDoie.Models.Scattergories;
using PatteDoie.Rows.Platform;
using PatteDoie.Rows.Scattegories;

namespace PatteDoie.Services.Scattergories
{
    public interface IScattegoriesService
    {
        public Task<IEnumerable<ScattegoriesGameRow>> GetAllGames();
        public Task<ScattegoriesGameRow> GetGame(Guid gameId);
        public Task<IEnumerable<ScattegoriesGameRow>> SearchGames(/*TODO*/);
        public Task UpdateGame(Guid gameId /*, TODO*/);
        public Task<ScattegoriesGameRow> NextRound(ScattergoriesGame game);
        public Task<ScattegoriesGameRow> CreateGame(int numberCategories, int roundNumber, List<User> users, User host);
        public Task DeleteGame(Guid gameId);
        public Task<ScattegoriesGameRow> EndScattergoriesGame(ScattergoriesGame game);
        public Task HostVerifyWord(ScattergoriesGame game, ScattergoriesPlayer player, ScattergoriesAnswer answer, bool decision);
    }
}
