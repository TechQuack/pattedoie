using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.Platform;
using PatteDoie.Rows.Scattegories;

namespace PatteDoie.Services.Scattergories
{
    public class ScattegoriesService : IScattegoriesService
    {
        private readonly PatteDoieContext _context;
        private readonly IMapper _mapper;

        public ScattegoriesService(PatteDoieContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ScattegoriesGameRow>> GetAllGames()
        {
            var games = (await _context.ScattergoriesGame.AsQueryable().ToListAsync());
            var result = new List<ScattegoriesGameRow>();
            foreach (var game in games)
            {
                result.Add(_mapper.Map<ScattegoriesGameRow>(game));
            }
            return result;
        }

        public async Task<ScattegoriesGameRow> GetGame(Guid gameId)
        {
            var game = (await _context.ScattergoriesGame.AsQueryable().Where(game => game.Id == gameId).ToListAsync()).FirstOrDefault();
            return _mapper.Map<ScattegoriesGameRow>(game);
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
