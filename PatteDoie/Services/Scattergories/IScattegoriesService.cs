using PatteDoie.Models.Platform;
using PatteDoie.Rows.Scattegories;

namespace PatteDoie.Services.Scattergories
{
    public interface IScattegoriesService
    {
        public Task<IEnumerable<ScattegoriesGameRow>> GetAllGames();
        public Task<ScattegoriesGameRow> GetGame(Guid gameId);
        public Task<IEnumerable<ScattegoriesGameRow>> SearchGames(/*TODO*/);
        public Task UpdateGame(Guid gameId /*, TODO*/);
        public Task<ScattegoriesGameRow> CreateGame(int roundNumber, List<PlatformUser> platformUsers);
        public Task DeleteGame(Guid gameId);
    }
}
