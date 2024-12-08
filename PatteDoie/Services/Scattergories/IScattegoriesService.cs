using PatteDoie.Models.Platform;
using PatteDoie.Models.Scattergories;
using PatteDoie.Rows.Scattegories;

namespace PatteDoie.Services.Scattergories
{
    public interface IScattegoriesService
    {
        public Task<IEnumerable<ScattegoriesGameRow>> GetAllGames();
        public Task<ScattegoriesGameRow> GetGame(Guid gameId);
        public Task<IEnumerable<ScattegoriesGameRow>> SearchGames(/*TODO*/);
        public Task UpdateGame(Guid gameId /*, TODO*/);
        public Task<ScattegoriesGameRow> AddPlayerWord(Guid gameId, Guid userId, string word, ScattergoriesCategory category);
        public Task<ScattegoriesGameRow> ConfirmWords(Guid gameId, Guid userId);
        public Task<ScattegoriesGameRow> NextRound(ScattergoriesGame game);
        public Task<ScattegoriesGameRow> CreateGame(int numberCategories, int roundNumber, Lobby lobby);
        public Task DeleteGame(Guid gameId);
        public Task<ScattegoriesGameRow> EndScattergoriesGame(ScattergoriesGame game);
        public Task<ScattegoriesGameRow> HostVerifyWord(Guid gameId, Guid playerId, Guid answerId, bool decision);
        public Task<List<ScattergoriesCategoryRow>> GetCategories(Guid gameId);
        public Task<List<ScattergoriesPlayerRow>> GetRank(Guid gameId);
    }
}
