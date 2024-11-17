using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.Platform;
using PatteDoie.Models.SpeedTyping;
using PatteDoie.Queries.SpeedTyping;
using PatteDoie.Rows.SpeedTypingGame;

namespace PatteDoie.Services.SpeedTyping
{
    public class SpeedTypingService : ISpeedTypingService
    {
        private readonly PatteDoieContext _context;

        private readonly IMapper _mapper;

        public SpeedTypingService(PatteDoieContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SpeedTypingGameRow> CreateGame(CreateSpeedTypingGameCommand command, PlatformUser[] platformUsers)
        {
            List<SpeedTypingScore> scores = [];
            foreach (PlatformUser platformUser in platformUsers)
            {
                var SpeedTypingScore = new SpeedTypingScore
                {
                    Score = 0,
                    UserId = platformUser.Id
                };
                scores.Add(SpeedTypingScore);
                _context.SpeedTypingScore.Add(SpeedTypingScore);
            }
            String ApiUrl = "https://random-word-api.herokuapp.com/word?lang=fr&number=10";
            String result = ApiCall.GetAsync(ApiUrl).Result.Remove(0, 1);
            result = result.Remove(result.Length - 1);
            String[] words = result.Replace("\"", "").Split(',');


            var speedTypingGame = new SpeedTypingGame
            {
                LaunchTime = DateTime.Now,
                Scores = [.. scores],
                Words = words,
                TimeProgresses = []
            };
            _context.SpeedTypingGame.Add(speedTypingGame);

            await _context.SaveChangesAsync();

            return _mapper.Map<SpeedTypingGameRow>(speedTypingGame);
        }

        public Task DeleteGame(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpeedTypingGameRow>> GetAllGames()
        {
            throw new NotImplementedException();
        }

        public async Task<SpeedTypingGameRow> GetGame(Guid gameId)
        {
            var game = (await _context.SpeedTypingGame.AsQueryable().Where(g => g.Id == gameId).ToListAsync())[0];
            return _mapper.Map<SpeedTypingGameRow>(game);
        }

        public Task<IEnumerable<SpeedTypingGameRow>> SearchGames(CreateSpeedTypingGameCommand query)
        {
            throw new NotImplementedException();
        }

        public Task UpdateGame(Guid id, CreateSpeedTypingGameCommand game)
        {
            throw new NotImplementedException();
        }
    }
}
