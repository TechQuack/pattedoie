using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PatteDoie.Hubs;
using PatteDoie.Migrations;
using PatteDoie.Models.Platform;
using PatteDoie.Models.Scattergories;
using PatteDoie.PatteDoieException;
using PatteDoie.Rows.Scattegories;

namespace PatteDoie.Services.Scattergories
{
    public class ScattegoriesService(IDbContextFactory<PatteDoieContext> factory, IMapper mapper, IHubContext<ScattergoriesHub> hub) : IScattegoriesService
    {
        private readonly static int TIME_BEFORE_DELETION = 60000;

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
               .Include(g => g.Players)
               .ThenInclude(p => p.Answers)
               .ThenInclude(a => a.Category)
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
            using var _context = _factory.CreateDbContext();
            var game = await _context.ScattergoriesGame.AsQueryable()
                .Include(g => g.Players)
                .ThenInclude(p => p.Answers)
                .Include(g => g.Categories)
                .FirstOrDefaultAsync(g => g.Id == gameId);
            var platformUser = await _context.PlatformUser.AsQueryable().FirstOrDefaultAsync(u => u.UserUUID == userId);
            var player = await _context.ScattergoriesPlayer.AsQueryable()
                .Include(p => p.Answers)
                .ThenInclude(a => a.Category)
                .FirstOrDefaultAsync(p => p.User == platformUser) ?? throw new PlayerNotValidException("Player not found");

            if (word.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(word));
            }
            if (word.Trim().IsNullOrEmpty())
            {
                throw new Exception($"invalid word(empty word)");
            }

            char letter = game.CurrentLetter;
            if (!(word.Trim().ToUpper().First() == letter))
            {
                throw new Exception($"invalid word(wrong first letter) - {word}");
            }

            foreach (var ans in player.Answers)
            {
                if (ans.Category.Name == category.Name)
                {
                    ans.Text = word;
                    _context.ScattergoriesAnswer.Update(ans);
                }
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<ScattegoriesGameRow>(game);
        }

        public async Task<ScattegoriesGameRow> ConfirmWords(Guid gameId, Guid userId)
        {
            using var _context = _factory.CreateDbContext();
            var game = await _context.ScattergoriesGame.AsQueryable()
                .Include(g => g.Players)
                .ThenInclude(p => p.Answers)
                .Include(g => g.Categories)
                .FirstOrDefaultAsync(g => g.Id == gameId) ?? throw new GameNotFoundException("Game not found");
            var platformUser = await _context.PlatformUser.AsQueryable().FirstOrDefaultAsync(u => u.UserUUID == userId);
            var player = await _context.ScattergoriesPlayer.AsQueryable()
                .Include(p => p.Answers)
                .ThenInclude(a => a.Category)
                .FirstOrDefaultAsync(p => p.User == platformUser) ?? throw new PlayerNotValidException("Player not found");
            if (HasCompletedCategories(player))
            {
                game.IsHostCheckingPhase = true;
                _context.ScattergoriesGame.Update(game);
                await _context.SaveChangesAsync();
                await _hub.Clients.Group(gameId.ToString())
                   .SendAsync("SendWords", gameId);
            }
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
                    DeletePlayerAnswers(player);
                    player.Answers = CreateEmptyAnswers(game.Categories).Result;
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
                var player = CreatePlayer(user, CreateEmptyAnswers(categories).Result, user == lobby.Creator);
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
            var game = await _context.ScattergoriesGame.AsQueryable()
                .Include(g => g.Players)
                .ThenInclude(p => p.Answers)
                .Include(g => g.Lobby)
               .FirstOrDefaultAsync<ScattergoriesGame>(g => g.Id == gameId) ?? throw new GameNotValidException("Scattergories game cannot be null");
            var lobby = await _context.PlatformLobby
                .Include(l => l.Users)
                .FirstOrDefaultAsync(l => l.Id == game.Lobby.Id) ?? throw new LobbyNotFoundException("Lobby not found");

            var users = lobby?.Users;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                foreach (var category in game.Categories)
                {
                    category.Games.Remove(game);
                }
                foreach (var player in game.Players)
                {
                    DeletePlayerAnswers(player);
                }
                _context.ScattergoriesPlayer.RemoveRange(game.Players);
                _context.ScattergoriesGame.Remove(game);
                _context.PlatformLobby.Remove(lobby);
                await _context.SaveChangesAsync();

                _context.PlatformUser.RemoveRange(users);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
        }

        public async Task<ScattegoriesGameRow> EndScattergoriesGame(ScattergoriesGame game)
        {
            if (!HasGameEnded(game))
            {
                throw new Exception("Scattergories game is not ended");
            }
            await UpdateHighScores(game.Id);
            Task deleteGame = this.DelayedDeletion(game.Id);

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

        public async Task<List<ScattergoriesPlayerRow>> GetPlayers(Guid gameId)
        {
            using var _context = _factory.CreateDbContext();
            var game = await _context.ScattergoriesGame.AsQueryable()
                .Include(g => g.Players)
                .ThenInclude(p => p.Answers)
                .ThenInclude(a => a.Category)
                .Include(g => g.Players)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync<ScattergoriesGame>(g => g.Id == gameId) ?? throw new GameNotValidException("Game not found");

            return _mapper.Map<List<ScattergoriesPlayerRow>>(game.Players);
        }

        public async Task<ScattergoriesPlayerRow> GetPlayerById(Guid id)
        {
            using var _context = _factory.CreateDbContext();
            var platformUser = _context.PlatformUser.AsQueryable().FirstOrDefault(u => u.UserUUID == id);
            var player = _context.ScattergoriesPlayer.AsQueryable()
                .Include(p => p.Answers)
                .ThenInclude(a => a.Category)
                .FirstOrDefault(p => p.User == platformUser) ?? throw new PlayerNotValidException("Player not found");
            _context.Dispose();
            return _mapper.Map<ScattergoriesPlayerRow>(player);
        }

        //TOOLS

        private async Task UpdateHighScores(Guid gameId)
        {
            using var _context = _factory.CreateDbContext();
            var game = await _context.ScattergoriesGame.AsQueryable().AsNoTracking()
                .Include(g => g.Players).ThenInclude(p => p.User)
                .Include(g => g.Lobby)
                .FirstOrDefaultAsync(g => g.Id == gameId) ?? throw new GameNotFoundException("Game not found");
            var lobby = await _context.PlatformLobby
               .Include(l => l.Game)
               .FirstOrDefaultAsync(l => l.Id == game.Lobby.Id) ?? throw new LobbyNotFoundException("Lobby cannot be null");
            var platformGame = await _context.PlatformGame.AsQueryable()
                .Include(p => p.HighScores)
                .FirstOrDefaultAsync(p => p.Name == "Scattergories") ?? throw new GameNotValidException("Game not valid");

            using var transaction = await _context.Database.BeginTransactionAsync();
            foreach (var player in game.Players)
            {
                var notExistingHighScore = await _context.PlatformHighScore.AsQueryable().FirstOrDefaultAsync(s => s.Id == player.Id) == null;
                if (notExistingHighScore)
                {
                    var highScore = new HighScore
                    {
                        Id = player.Id,
                        Score = player.Score,
                        PlayerName = player.User.Nickname
                    };
                    await _context.PlatformHighScore.AddAsync(highScore);
                    platformGame.HighScores.Add(highScore);
                }
            }

            var highScoresToDelete = platformGame.HighScores.OrderByDescending(h => h.Score).Skip(5).ToList();
            platformGame.HighScores = platformGame.HighScores.OrderByDescending(h => h.Score).Take(5).ToList();
            _context.PlatformHighScore.RemoveRange(highScoresToDelete);

            _context.PlatformGame.Update(platformGame);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        private static ScattergoriesPlayer CreatePlayer(User player, List<ScattergoriesAnswer> answers, bool isHost)
        {
            return new ScattergoriesPlayer
            {
                Score = 0,
                User = player,
                Answers = answers,
                IsHost = isHost
            };
        }

        private async Task<List<ScattergoriesAnswer>> CreateEmptyAnswers(List<ScattergoriesCategory> categories)
        {
            using var _context = _factory.CreateDbContext();
            List<ScattergoriesAnswer> answers = [];
            foreach (var category in categories)
            {
                var answer = new ScattergoriesAnswer
                {
                    Text = "",
                    Category = category,
                    IsChecked = false
                };
                answers.Add(answer);
                await _context.ScattergoriesAnswer.AddAsync(answer);
            }
            return answers;
        }

        private void DeletePlayerAnswers(ScattergoriesPlayer player)
        {
            using var _context = _factory.CreateDbContext();
            foreach (var answer in player.Answers)
            {
                _context.Remove(answer);
            }
            player.Answers = [];
        }

        private static bool HasGameEnded(ScattergoriesGame game)
        {
            return game.CurrentRound == game.MaxRound;
        }

        private async Task DelayedDeletion(Guid gameId)
        {
            await Task.Delay(TIME_BEFORE_DELETION);
            await DeleteGame(gameId);
            await _hub.Clients.Group(gameId.ToString()).SendAsync("RedirectToHome", gameId);
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

        private static bool HasCompletedCategories(ScattergoriesPlayer player)
        {
            var res = true;
            foreach (var answer in player.Answers)
            {
                if (answer.Text == "")
                {
                    res = false;
                }
            }
            return res;
        }

        private static char RandomLetter()
        {
            return (char)new Random().Next(65, 90);
        }
    }
}
