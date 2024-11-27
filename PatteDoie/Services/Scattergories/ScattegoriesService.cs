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
            var games = await _context.ScattergoriesGame.AsQueryable().ToListAsync();
            return _mapper.Map<List<ScattegoriesGameRow>>(games);
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

        public async Task<ScattegoriesGameRow> CreateGame(int numberCategories, int roundNumber, List<User> users, User host)
        {
            var rand = new Random();

            var potentialsCategories = (await _context.ScattergoriesCategory.AsQueryable().ToListAsync());
            List<ScattergoriesCategory> categories = potentialsCategories.OrderBy(x => rand.Next()).Take(numberCategories).ToList();

            var players = new List<ScattergoriesPlayer>();
            foreach (var user in users)
            {
                var playerAnswers = CreateEmptyAnswers(categories);
                var player = CreatePlayer(user, playerAnswers, false);
                players.Add(player);
                _context.ScattergoriesPlayer.Add(player);
            }
            var hostAnswers = CreateEmptyAnswers(categories);
            var hostPlayer = CreatePlayer(host, hostAnswers, true);
            players.Add(hostPlayer);
            _context.ScattergoriesPlayer.Add(hostPlayer);

            char letter = (char)rand.Next(65, 90);

            var game = new ScattergoriesGame
            {
                Players = players,
                MaxRound = roundNumber,
                CurrentRound = 1,
                CurrentLetter = letter,
                Categories = categories
            };
            _context.ScattergoriesGame.Add(game);

            await _context.SaveChangesAsync();

            return _mapper.Map<ScattegoriesGameRow>(game);
        }

        public Task DeleteGame(Guid gameId)
        {
            throw new NotImplementedException();
        }

        //TOOLS

        private ScattergoriesPlayer CreatePlayer(User player, List<ScattegoriesAnswer> answers, bool isHost)
        {
            return new ScattergoriesPlayer
            {
                Score = 0,
                User = player,
                Answers = answers,
                IsHost = isHost
            };
        }

        private List<ScattegoriesAnswer> CreateEmptyAnswers(List<ScattergoriesCategory> categories)
        {
            var answers = new List<ScattegoriesAnswer>();
            foreach (var category in categories)
            {
                var answer = new ScattegoriesAnswer
                {
                    Text = "",
                    Category = category
                };
                answers.Add(answer);
                _context.ScattegoriesAnswer.Add(answer);
            }
            return answers;
        }
    }
}
