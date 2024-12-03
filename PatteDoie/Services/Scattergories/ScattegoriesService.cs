using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PatteDoie.Models.Platform;
using PatteDoie.Models.Scattergories;
using PatteDoie.PatteDoieException;
using PatteDoie.Rows.Scattegories;

namespace PatteDoie.Services.Scattergories
{
    public class ScattegoriesService(IDbContextFactory<PatteDoieContext> factory, IMapper mapper) : IScattegoriesService
    {
        public static int TIME_BEFORE_DELETION = 60000;

        private readonly IDbContextFactory<PatteDoieContext> _factory = factory;
        private readonly IMapper _mapper = mapper;
        private readonly NavigationManager NavigationManager = default!;

        public async Task<IEnumerable<ScattegoriesGameRow>> GetAllGames()
        {
            using var _context = _factory.CreateDbContext();
            var games = await _context.ScattergoriesGame.AsQueryable().ToListAsync();
            return _mapper.Map<List<ScattegoriesGameRow>>(games);
        }

        public async Task<ScattegoriesGameRow> GetGame(Guid gameId)
        {
            using var _context = _factory.CreateDbContext();
            var game = (_context.ScattergoriesGame.AsQueryable()
               .Include(g => g.Lobby)
               .ThenInclude(l => l.Users)
               .FirstOrDefault(g => g.Id == gameId)) ?? throw new GameNotValidException("Scattergories game cannot be null");
            await _context.DisposeAsync();
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

        public async Task<ScattegoriesGameRow> AddPlayerWord(ScattergoriesGame game, ScattergoriesPlayer player, string word, ScattergoriesCategory category)
        {
            if (word.Trim().IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(word));
            }
            char letter = game.CurrentLetter;
            if (!word.Trim().First().Equals(letter))
            {
                throw new Exception($"invalid word(wrong first letter)  - {word}");
            }
            ScattergoriesAnswer? ExistingAnswer = null;
            foreach (var ans in player.Answers)
            {
                if (ans.Category == category)
                {
                    ExistingAnswer = ans;
                }
            }
            if (ExistingAnswer != null)
            {
                ExistingAnswer.Text = word;
            } else
            {
                ScattergoriesAnswer answer = new ScattergoriesAnswer
                {
                    Category = category,
                    Text = word,
                    IsChecked = false
                };
                player.Answers.Add(answer);
            }
            if (HasCompletedCategories(player, game) && !game.IsHostCheckingPhase)
            {
                game.IsHostCheckingPhase = true;
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<ScattegoriesGameRow>(game);
        }

        public async Task<ScattegoriesGameRow> NextRound(ScattergoriesGame game)
        {
            using var _context = _factory.CreateDbContext();
            if (HasGameEnded(game))
            {
                return EndScattergoriesGame(game).Result;
            }
            else
            {
                game.CurrentLetter = RandomLetter();
                game.CurrentRound += 1;
                game.IsHostCheckingPhase = false;
                foreach (var player in game.Players)
                {
                    player.Answers = new List<ScattergoriesAnswer>();
                }
                await _context.SaveChangesAsync();
                await _context.DisposeAsync();
                return _mapper.Map<ScattegoriesGameRow>(game);
            }
        }

        public async Task<ScattegoriesGameRow> CreateGame(int numberCategories, int roundNumber, Lobby lobby)
        {
            using var _context = _factory.CreateDbContext();
            await _context.Entry(lobby).ReloadAsync();
            lobby.Users.ForEach(async u => await _context.Entry(u).ReloadAsync());
            var rand = new Random();

            var potentialsCategories = (await _context.ScattergoriesCategory.AsQueryable().ToListAsync());
            List<ScattergoriesCategory> categories = potentialsCategories.OrderBy(x => rand.Next()).Take(numberCategories).ToList();
            
            var players = new List<ScattergoriesPlayer>();
            foreach (var user in lobby.Users)
            {
                var playerAnswers = new List<ScattergoriesAnswer>();
                var player = CreatePlayer(user, playerAnswers, false);
                players.Add(player);
                _context.ScattergoriesPlayer.Add(player);
            }
            var hostAnswers = new List<ScattergoriesAnswer>();
            var hostPlayer = CreatePlayer(lobby.Creator, hostAnswers, true);
            players.Add(hostPlayer);
            _context.ScattergoriesPlayer.Add(hostPlayer);

            var game = new ScattergoriesGame
            {
                Players = players,
                MaxRound = roundNumber,
                CurrentRound = 1,
                CurrentLetter = RandomLetter(),
                Categories = categories,
                IsHostCheckingPhase = false,
                Lobby = lobby
            };
            _context.ScattergoriesGame.Add(game);

            foreach (var category in categories)
            {
                category.Games.Add(game);
            }

            await _context.SaveChangesAsync();
            await _context.DisposeAsync();
            return _mapper.Map<ScattegoriesGameRow>(game);
        }

        public async Task DeleteGame(Guid gameId)
        {
            using var _context = _factory.CreateDbContext();
            var game = _context.ScattergoriesGame.AsQueryable()
               .Where(g => g.Id == gameId)
               .FirstOrDefault<ScattergoriesGame>() ?? throw new GameNotValidException("Scattergories game cannot be null");
            foreach (var category in  game.Categories)
            {
                category.Games.Remove(game);
            }
            _context.ScattergoriesPlayer.RemoveRange(game.Players);
            _context.ScattergoriesGame.Remove(game);
            await _context.SaveChangesAsync();
            await _context.DisposeAsync();
        }

        public async Task<ScattegoriesGameRow> EndScattergoriesGame(ScattergoriesGame game)
        {
            if (!HasGameEnded(game))
            {
                throw new Exception("Scattergories game is not ended");
            }

            Task deleteGame = this.DelayedDeletion(game);

            return _mapper.Map<ScattegoriesGameRow>(game);
        }

        public async Task<ScattegoriesGameRow> HostVerifyWord(ScattergoriesGame game, ScattergoriesPlayer player, ScattergoriesAnswer answer, bool decision)
        {
            using var _context = _factory.CreateDbContext();
            if (decision)
            {
                player.Score += 1;
            }
            answer.IsChecked = true;
            _context.ScattergoriesPlayer.Update(player);
            await _context.SaveChangesAsync();

            if (AreAllWordsChecked(game))
            {
                return NextRound(game).Result;
            }
            await _context.DisposeAsync();
            return _mapper.Map<ScattegoriesGameRow>(game);
        }

        //TOOLS

        private ScattergoriesPlayer CreatePlayer(User player, List<ScattergoriesAnswer> answers, bool isHost)
        {
            return new ScattergoriesPlayer
            {
                Score = 0,
                User = player,
                Answers = answers,
                IsHost = isHost
            };
        }

        private static bool HasGameEnded(ScattergoriesGame game)
        {
            return game.CurrentRound == game.MaxRound;
        }

        private async Task DelayedDeletion(ScattergoriesGame game)
        {
            await Task.Delay(TIME_BEFORE_DELETION);
            await DeleteGame(game.Id);
            NavigationManager.NavigateTo("/home");
        }


        private static bool AreAllWordsChecked(ScattergoriesGame game)
        {
            var players = game.Players;
            foreach (var player in players)
            {
                var answers = player.Answers;
                foreach (var answer in answers)
                {
                    if (!answer.IsChecked)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool HasCompletedCategories(ScattergoriesPlayer player, ScattergoriesGame game)
        {
            List<ScattergoriesCategory> categoriesAnswered = new List<ScattergoriesCategory>();
            foreach (var answer in player.Answers)
            {
                if (answer.Text.Trim().IsNullOrEmpty())
                {
                    return false;
                }
                categoriesAnswered.Add(answer.Category);
            }
            return Enumerable.SequenceEqual(game.Categories.OrderBy(x => x), categoriesAnswered.OrderBy(x => x));
        }

        private static char RandomLetter()
        {
            return (char)new Random().Next(65, 90);
        }
    }
}
