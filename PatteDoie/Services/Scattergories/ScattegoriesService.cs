using PatteDoie.Models.Platform;
using PatteDoie.Rows.Scattegories;

namespace PatteDoie.Services.Scattergories
{
    public class ScattegoriesService : IScattegoriesService
    {
        private readonly PatteDoieContext _context;

        public ScattegoriesService(PatteDoieContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<ScattegoriesGameRow>> GetAllGames()
        {
            throw new NotImplementedException();
        }

        public Task<ScattegoriesGameRow> GetGame(Guid gameId)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<ScattegoriesGameRow>> SearchGames()
        {
            throw new NotImplementedException();
        }

        public Task UpdateGame(Guid gameId)
        {
            throw new NotImplementedException();
        }

        public Task<ScattegoriesGameRow> CreateGame(PlatformUser[] platformUsers)
        {
            throw new NotImplementedException();
        }

        public Task DeleteGame(Guid gameId)
        {
            throw new NotImplementedException();
        }
    }
}
