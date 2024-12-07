using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Hubs;
using PatteDoie.Models.Platform;
using PatteDoie.Models.SpeedTyping;
using PatteDoie.PatteDoieException;
using PatteDoie.Queries.SpeedTyping;
using PatteDoie.Rows.SpeedTyping;
using PatteDoie.Rows.SpeedTypingGame;

namespace PatteDoie.Services.SpeedTyping
{
    public class SpeedTypingService : ISpeedTypingService
    {
        public static int TIME_BEFORE_DELETION = 60000;
        public static int TIME_BEFORE_ENDING = 60000;

        private readonly IDbContextFactory<PatteDoieContext> _factory;

        private readonly IMapper _mapper;
        private readonly IHubContext<SpeedTypingHub> _hub;

        public SpeedTypingService(IDbContextFactory<PatteDoieContext> factory, IMapper mapper, IHubContext<SpeedTypingHub> hub)
        {
            _factory = factory;
            _mapper = mapper;
            _hub = hub;
        }

        public async Task<SpeedTypingGameRow> CreateGame(Lobby lobby)
        {
            using var _context = _factory.CreateDbContext();
            await _context.Entry(lobby).ReloadAsync();
            lobby.Users.ForEach(async u => await _context.Entry(u).ReloadAsync());
            List<SpeedTypingPlayer> players = [];
            foreach (User platformUser in lobby.Users)
            {
                var speedTypingPlayer = new SpeedTypingPlayer
                {
                    Score = 0,
                    User = platformUser
                };
                players.Add(speedTypingPlayer);
                _context.SpeedTypingPlayer.Add(speedTypingPlayer);
            }
            String ApiUrl = "https://random-word-api.herokuapp.com/word?lang=en&number=10";
            String result = await ApiCall.GetAsync(ApiUrl);
            result = result.Remove(0, 1);
            result = result.Remove(result.Length - 1);
            String[] words = result.Replace("\"", "").Split(',');

            var speedTypingGame = new SpeedTypingGame
            {
                LaunchTime = DateTime.Now,
                Players = players,
                Words = new List<string>(words),
                TimeProgresses = [],
                Lobby = lobby
            };
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                await _context.SpeedTypingGame.AddAsync(speedTypingGame);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            Task endGame = this.DelayedEnding(speedTypingGame);
            return _mapper.Map<SpeedTypingGameRow>(speedTypingGame);
        }

        public async Task DeleteGame(Guid id)
        {
            using var _context = _factory.CreateDbContext();
            var game = _context.SpeedTypingGame
                .Include(g => g.Players)
                .Include(g => g.TimeProgresses)
                .Include(g => g.Lobby)
                .FirstOrDefault(g => g.Id == id)
                ?? throw new GameNotValidException("Speed typing game cannot be null");

            var lobby = _context.PlatformLobby
                .Include(l => l.Users)
                .Include(l => l.Creator)
                .FirstOrDefault(l => l.Id == game.Lobby.Id)
                ?? throw new LobbyNotFoundException("Lobby cannot be null");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                _context.SpeedTypingPlayer.RemoveRange(game.Players);
                _context.SpeedTypingTimeProgress.RemoveRange(game.TimeProgresses);
                _context.SpeedTypingGame.Remove(game);

                _context.PlatformLobby.Remove(lobby);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }

        public Task<IEnumerable<SpeedTypingGameRow>> GetAllGames()
        {
            throw new NotImplementedException();
        }

        public async Task<SpeedTypingGameRow> GetGame(Guid gameId)
        {
            using var _context = _factory.CreateDbContext();
            var game = await _context.SpeedTypingGame.AsQueryable()
                .Include(g => g.Lobby)
                .ThenInclude(l => l.Users)
                .FirstOrDefaultAsync(g => g.Id == gameId);
            await _context.DisposeAsync();
            return _mapper.Map<SpeedTypingGameRow>(game);
        }

        public Task<IEnumerable<SpeedTypingGameRow>> SearchGames(CreateSpeedTypingGameCommand query)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetScore(Guid playerUUID)
        {
            using var _context = _factory.CreateDbContext();
            var platformUser = await _context.PlatformUser.AsQueryable().Where(u => u.UserUUID == playerUUID).FirstOrDefaultAsync();
            var player = await _context.SpeedTypingPlayer.AsQueryable().Where(p => p.User == platformUser).FirstOrDefaultAsync()
                ?? throw new PlayerNotValidException("Player not found");
            await _context.DisposeAsync();
            return player.Score;
        }


        public async Task<bool> CanPlay(Guid playerId, Guid gameId)
        {
            using var _context = _factory.CreateDbContext();
            var game = _context.SpeedTypingGame.AsQueryable()
               .Where(g => g.Id == gameId)
               .FirstOrDefault<SpeedTypingGame>();
            if (game == null)
            {
                return false;
            }
            var platformUser = await _context.PlatformUser.AsQueryable().Where(u => u.UserUUID == playerId).FirstOrDefaultAsync();
            var player = await _context.SpeedTypingPlayer.AsQueryable().Where(p => p.User == platformUser).FirstOrDefaultAsync()
                ?? throw new PlayerNotValidException("Player not found");

            var launch = game.LaunchTime;
            var now = DateTime.UtcNow;
            var elaspedTime = (now - launch).TotalMilliseconds;
            if (HasFinished(game, player))
            {
                return false;
            }
            return elaspedTime < TIME_BEFORE_ENDING;
        }

        public async Task ManageEndOfGame(Guid gameId)
        {
            using var _context = _factory.CreateDbContext();
            var game = _context.SpeedTypingGame.AsQueryable()
               .Where(g => g.Id == gameId)
               .FirstOrDefault<SpeedTypingGame>();
            if (game is null)
            {
                throw new GameNotValidException("Speed typing game cannot be null");
            }
            var players = await _context.SpeedTypingGame
                .Where(g => g.Id == game.Id)
                .SelectMany(game => game.Players).ToListAsync();
            foreach (SpeedTypingPlayer player in players)
            {
                if (!HasFinished(game, player) && player.SecondsToFinish == 0)
                {
                    await SetTimeProgress(game, player, DateTime.UtcNow);
                }
            }
            await _hub.Clients.Group(gameId.ToString())
                    .SendAsync("ShowRanking", gameId);
            await UpdateHighscores(gameId);
            Task deleteGame = this.DelayedDeletion(game);
            await _context.DisposeAsync();
        }
        public async Task SetTimeProgress(SpeedTypingGame game, SpeedTypingPlayer player, DateTime timeProgress)
        {
            using var _context = _factory.CreateDbContext();
            if (player is null)
            {
                throw new PlayerNotValidException("Speed typing player cannot be null");
            }
            if (game is null)
            {
                throw new GameNotValidException("Speed typing game cannot be null");
            }
            player.SecondsToFinish = (int)(DateTime.UtcNow - game.LaunchTime).TotalSeconds;
            _context.SpeedTypingPlayer.Update(player);
            SpeedTypingTimeProgress playerProgress = new()
            {
                Player = player,
                TimeProgress = timeProgress
            };
            _context.SpeedTypingTimeProgress.Update(playerProgress);

            game.TimeProgresses.Add(playerProgress);
            _context.SpeedTypingGame.Update(game);

            await _context.SaveChangesAsync();
            await _context.DisposeAsync();
        }

        public async Task<bool> CheckWord(Guid gameId, Guid uuid, string word)
        {

            using var _context = _factory.CreateDbContext();
            if (!await CanPlay(uuid, gameId))
            {
                await _context.DisposeAsync();
                return false;
            }
            var game = _context.SpeedTypingGame.AsQueryable()
                .Where(g => g.Id == gameId)
                .FirstOrDefault<SpeedTypingGame>() ?? throw new GameNotValidException("Game not found");
            var platformUser = await _context.PlatformUser.AsQueryable().Where(u => u.UserUUID == uuid).FirstOrDefaultAsync();
            var player = await _context.SpeedTypingPlayer.AsQueryable().Where(p => p.User == platformUser).FirstOrDefaultAsync()
                ?? throw new PlayerNotValidException("Player not found");
            var wordIndexToCheck = player.Score;
            var wordToCheck = game.Words[wordIndexToCheck];

            if (wordToCheck == word)
            {
                player.Score += 1;
                _context.SpeedTypingPlayer.Update(player);
                await _context.SaveChangesAsync();
                await _hub.Clients.Group(gameId.ToString())
                    .SendAsync("ReceiveProgression", _mapper.Map<SpeedTypingPlayerRow>(player));

                if (HasFinished(game, player))
                {
                    await SetTimeProgress(game, player, DateTime.UtcNow);
                    if (await AllPlayersHaveFinished(game))
                    {
                        await ManageEndOfGame(gameId);
                    }
                }
                await _context.DisposeAsync();
                return true;
            }
            else
            {
                await _context.DisposeAsync();
                return false;
            }

        }

        public async Task<List<SpeedTypingPlayerRow>> GetRank(Guid gameId)
        {
            using var _context = _factory.CreateDbContext();
            var game = await _context.SpeedTypingGame.AsQueryable()
                .Include(g => g.Players).ThenInclude(p => p.User)
                .FirstOrDefaultAsync<SpeedTypingGame>(g => g.Id == gameId) ?? throw new GameNotValidException("Game not found");
            await _context.DisposeAsync();
            return _mapper.Map<List<SpeedTypingPlayerRow>>(game.Players
                .OrderByDescending(player => player.Score)
                .ThenBy(player => GetTimeProgress(player))
                );
        }

        private async Task<bool> AllPlayersHaveFinished(SpeedTypingGame game)
        {
            using var _context = _factory.CreateDbContext();
            var players = await _context.SpeedTypingGame
                .Where(g => g.Id == game.Id)
                .SelectMany(game => game.Players).ToListAsync();
            return players.All(p => HasFinished(game, p));
        }

        private bool HasFinished(SpeedTypingGame game, SpeedTypingPlayer player)
        {
            return player.Score == game.Words.Count;
        }

        private async Task DelayedDeletion(SpeedTypingGame game)
        {
            await Task.Delay(TIME_BEFORE_DELETION);
            await DeleteGame(game.Id);
            await _hub.Clients.Group(game.Id.ToString())
                    .SendAsync("RedirectToHome", game.Id);
        }

        private async Task DelayedEnding(SpeedTypingGame game)
        {
            await Task.Delay(TIME_BEFORE_ENDING);
            await ManageEndOfGame(game.Id);
        }

        private DateTime? GetTimeProgress(SpeedTypingPlayer player)
        {
            using var _context = _factory.CreateDbContext();
            var timeProgress = _context.SpeedTypingTimeProgress
                .AsQueryable()
                .Where(s => s.Player == player).FirstOrDefault() ?? null;
            return timeProgress?.TimeProgress;
        }

        private async Task UpdateHighscores(Guid gameId)
        {
            using var _context = _factory.CreateDbContext();

            var game = await _context.SpeedTypingGame.AsQueryable().AsNoTracking()
                .Include(g => g.Players).ThenInclude(p => p.User)
                .Include(g => g.Lobby)
                .FirstOrDefaultAsync<SpeedTypingGame>(g => g.Id == gameId) ?? throw new GameNotValidException("Game not found");
            var lobby = await _context.PlatformLobby
               .Include(l => l.Game)
               .FirstOrDefaultAsync(l => l.Id == game.Lobby.Id)
               ?? throw new LobbyNotFoundException("Lobby cannot be null");

            var platformGame = await _context.PlatformGame
                .Include(l => l.HighScores)
                .FirstOrDefaultAsync(p => p.Name == "SpeedTyping")
                ?? throw new GameNotValidException("Game not valid");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                foreach (var p in game.Players)
                {
                    var existingHighScore = await _context.PlatformHighScore
                         .FirstOrDefaultAsync(h => h.Id == p.Id);
                    if (existingHighScore == null)
                    {
                        var highScore = new HighScore
                        {
                            Id = p.Id,
                            Score = p.Score * 60 / p.SecondsToFinish,
                            PlayerName = p.User.Nickname
                        };
                        await _context.PlatformHighScore.AddAsync(highScore);
                        platformGame.HighScores.Add(highScore);
                    }
                }
                platformGame.HighScores = platformGame.HighScores.OrderByDescending(h => h.Score).Take(5).ToList();
                var highScoresToDelete = platformGame.HighScores.OrderByDescending(h => h.Score).Skip(5).ToList();

                _context.PlatformHighScore.RemoveRange(highScoresToDelete);

                _context.PlatformGame.Update(platformGame);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }

    }
}
