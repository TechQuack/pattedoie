using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.Platform;
using PatteDoie.Models.Scattergories;
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

        public async Task<ScattegoriesGameRow> CreateGame(int numberCategories, int roundNumber, List<PlatformUser> platformUsers)
        {
            var players = new List<ScattergoriesPlayer>();
            foreach (var platformUser in platformUsers)
            {
                var ScattegoriesPlayer = new ScattergoriesPlayer
                {
                    Score = 0,
                    User = platformUser
                };
                players.Add(ScattegoriesPlayer);
                _context.ScattergoriesPlayer.Add(ScattegoriesPlayer);
            }

            var rand = new Random();
            char letter = (char)rand.Next(65, 90);

            var categories = new List<ScattergoriesCategory>(); /*TODO*/

            var game = new ScattergoriesGame
            {
                Players = [.. players],
                MaxRound = roundNumber,
                CurrentRound = 1,
                CurrentLetter = letter,
                Categories = [.. categories]
            };
            _context.ScattergoriesGame.Add(game);

            await _context.SaveChangesAsync();

            return _mapper.Map<ScattegoriesGameRow>(game);
        }

        public Task DeleteGame(Guid gameId)
        {
            throw new NotImplementedException();
        }
    }
}
