using PatteDoie.Models.Platform;
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
        public Task<ScattegoriesGameRow> CreateGame(int numberCategories, int roundNumber, List<User> users, User host);
        public Task DeleteGame(Guid gameId);
        public Task<PlatformUserRow> EndScattergoriesGame(Guid gameId);
    }
}
