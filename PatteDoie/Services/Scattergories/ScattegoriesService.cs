using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PatteDoie.Hubs;
using PatteDoie.Models.Platform;
using PatteDoie.Models.Scattergories;
using PatteDoie.PatteDoieException;
using PatteDoie.Rows.Scattegories;

namespace PatteDoie.Services.Scattergories
{
    public class ScattegoriesService(IDbContextFactory<PatteDoieContext> factory, IMapper mapper, IHubContext<ScattergoriesHub> hub) : IScattegoriesService
    {
        public static int TIME_BEFORE_DELETION = 60000;

        private readonly IDbContextFactory<PatteDoieContext> _factory = factory;
        private readonly IMapper _mapper = mapper;
        private readonly NavigationManager NavigationManager = default!;

        private readonly IHubContext<ScattergoriesHub> _hub = hub;

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
               .Include(g => g.Categories)
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

        public async Task<ScattegoriesGameRow> AddPlayerWord(Guid gameId, Guid userId, string word, ScattergoriesCategory category)
        {
            Console.WriteLine("_________________________________________________2.1");
            using var _context = _factory.CreateDbContext();
            var game = await _context.ScattergoriesGame.AsQueryable()
                .Include(g => g.Players)
                .ThenInclude(p => p.Answers)
                .Include(g => g.Categories)
                .FirstOrDefaultAsync(g => g.Id == gameId);
            Console.WriteLine("_________________________________________________2.2");
            var platformUser = await _context.PlatformUser.AsQueryable().FirstOrDefaultAsync(u => u.UserUUID == userId);
            var player = await _context.ScattergoriesPlayer.AsQueryable()
                .Include(p => p.Answers)
                .ThenInclude(a => a.Category)
                .FirstOrDefaultAsync(p => p.User == platformUser) ?? throw new PlayerNotValidException("Player not found");
            Console.WriteLine($"_________________________________________________2.3 {word}");
            if (word.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(word));
            }
            if (word.Trim().IsNullOrEmpty())
            {
                throw new Exception($"invalid word(empty word)");
            }
            Console.WriteLine($"_________________________________________________2.4: {word.Trim().ToUpper().First()}, {game.CurrentLetter}");
            char letter = game.CurrentLetter;
            if (!(word.Trim().ToUpper().First() == letter))
            {
                throw new Exception($"invalid word(wrong first letter) - {word}");
            }
            Console.WriteLine("_________________________________________________2.5");
            ScattergoriesAnswer? ExistingAnswer = null;
            Console.WriteLine(player == null);
            foreach (var ans in player.Answers)
            {
                if (ans.Category == category)
                {
                    ExistingAnswer = ans;
                }
            }
            Console.WriteLine("_________________________________________________2.6");
            if (ExistingAnswer != null)
            {
                ExistingAnswer.Text = word;
            }
            else
            {
                Console.WriteLine("____________________________________________________2.6.1");
                Console.WriteLine($"catégorie: {category.Name}");
                ScattergoriesAnswer answer = new ScattergoriesAnswer
                {
                    Id = Guid.NewGuid(),
                    Category = category,
                    Text = word,
                    IsChecked = false
                };
                Console.WriteLine("____________________________________________________2.6.2");
                await _context.ScattergoriesAnswer.AddAsync(answer);
                Console.WriteLine("____________________________________________________2.6.3");
                player.Answers.Add(answer);
            }
            Console.WriteLine("_________________________________________________2.7");
            if (HasCompletedCategories(player, game) && !game.IsHostCheckingPhase)
            {
                game.IsHostCheckingPhase = true;
            }
            Console.WriteLine("_________________________________________________2.8");
            _context.ScattergoriesPlayer.Update(player);
            Console.WriteLine("_________________________________________________2.9");
            await _context.SaveChangesAsync();
            Console.WriteLine("_________________________________________________2.10");
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
                var player = CreatePlayer(user, playerAnswers, user == lobby.Creator);
                players.Add(player);
                _context.ScattergoriesPlayer.Add(player);
            }

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
            foreach (var category in game.Categories)
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

        public async Task<ScattegoriesGameRow> HostVerifyWord(Guid gameId, Guid playerId, Guid answerId, bool decision)
        {
            using var _context = _factory.CreateDbContext();
            var player = await _context.ScattergoriesPlayer.AsQueryable()
                .Where(p => p.Id == playerId)
                .Include(p => p.Answers)
                .FirstOrDefaultAsync() ?? throw new Exception("Player does not exists");
            var answer = player.Answers.Find(a => a.Id == answerId) ?? throw new Exception("Answer not found");
            if (decision)
            {
                player.Score += 1;
                _context.ScattergoriesPlayer.Update(player);
            }
            answer.IsChecked = true;
            _context.ScattergoriesAnswer.Update(answer);
            await _context.SaveChangesAsync();

            var game = _context.ScattergoriesGame.AsQueryable()
                .Where(g => g.Id == gameId)
                .FirstOrDefault<ScattergoriesGame>() ?? throw new GameNotValidException("Game not found");
            await _context.DisposeAsync();
            await _hub.Clients.Group(gameId.ToString())
                    .SendAsync("ReceiveProgression", _mapper.Map<ScattergoriesPlayerRow>(player));
            if (AreAllWordsChecked(game))
            {
                return NextRound(game).Result;
            }

            return _mapper.Map<ScattegoriesGameRow>(game);
        }

        public async Task<List<ScattergoriesCategoryRow>> GetCategories(Guid gameId)
        {
            using var _context = _factory.CreateDbContext();
            var game = await _context.ScattergoriesGame.AsQueryable()
                .FirstOrDefaultAsync<ScattergoriesGame>(g => g.Id == gameId) ?? throw new GameNotValidException("Game not found");

            return _mapper.Map<List<ScattergoriesCategoryRow>>(game.Categories);
        }

        public async Task<List<ScattergoriesPlayerRow>> GetRank(Guid gameId)
        {
            using var _context = _factory.CreateDbContext();
            var game = await _context.ScattergoriesGame.AsQueryable()
                .Include(g => g.Players).ThenInclude(p => p.User)
                .FirstOrDefaultAsync<ScattergoriesGame>(g => g.Id == gameId) ?? throw new GameNotValidException("Game not found");
            return _mapper.Map<List<ScattergoriesPlayerRow>>(game.Players
                .OrderByDescending(player => player.Score)
            );
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
            return player.Answers.Count == game.Categories.Count;
        }

        private static char RandomLetter()
        {
            return (char)new Random().Next(65, 90);
        }
    }
}
