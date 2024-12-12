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
        public Task<ScattergoriesGameRow> AddPlayerWord(Guid gameId, Guid userId, string word, ScattergoriesCategory category);
        public Task<ScattergoriesGameRow> ConfirmWords(Guid gameId, Guid userId);
        public Task<ScattergoriesGameRow> CreateGame(int numberCategories, int roundNumber, Lobby lobby);
        public Task DeleteGame(Guid gameId);
        public Task<ScattergoriesGameRow> EndScattergoriesGame(ScattergoriesGame game);
        public Task<ScattergoriesGameRow> HostVerifyWord(Guid gameId, Guid playerId, Guid answerId, bool decision);
        public Task<List<ScattergoriesCategoryRow>> GetCategories(Guid gameId);
        public Task<List<ScattergoriesPlayerRow>> GetRank(Guid gameId);
        public Task<List<ScattergoriesPlayerRow>> GetPlayers(Guid gameId);
        public Task<ScattergoriesPlayerRow> GetPlayerById(Guid id);
    }
}
