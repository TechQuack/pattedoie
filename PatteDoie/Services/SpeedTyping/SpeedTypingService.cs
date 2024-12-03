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
            String ApiUrl = "https://random-word-api.herokuapp.com/word?lang=fr&number=10";
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
            _context.SpeedTypingGame.Add(speedTypingGame);
            await _context.SaveChangesAsync();
            Task endGame = this.DelayedEnding(speedTypingGame);
            await _context.DisposeAsync();

            return _mapper.Map<SpeedTypingGameRow>(speedTypingGame);
        }

        public async Task DeleteGame(Guid id)
        {
            using var _context = _factory.CreateDbContext();
            var game = _context.SpeedTypingGame
                .Include(g => g.Players)
                .Include(g => g.TimeProgresses)
                .FirstOrDefault(g => g.Id == id)
                ?? throw new GameNotValidException("Speed typing game cannot be null");
            _context.SpeedTypingPlayer.RemoveRange(game.Players);
            _context.SpeedTypingTimeProgress.RemoveRange(game.TimeProgresses);
            _context.SpeedTypingGame.Remove(game);
            await _context.SaveChangesAsync();
            await _context.DisposeAsync();
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
            foreach (SpeedTypingPlayer player in game.Players)
            {
                if (!HasFinished(game, player))
                {
                    await SetTimeProgress(game, player, DateTime.UtcNow);
                }
            }
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

                if (player.Score == game.Words.Count)
                {
                    await SetTimeProgress(game, player, DateTime.UtcNow);
                }
                if (allPlayersHaveFinished(game))
                {
                    await ManageEndOfGame(gameId);
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
            return _mapper.Map<List<SpeedTypingPlayerRow>>(game.Players.OrderByDescending(player => player.Score));
        }

        private bool allPlayersHaveFinished(SpeedTypingGame game)
        {
            using var _context = _factory.CreateDbContext();
            var players = _context.SpeedTypingGame
                .Where(game => game.Id == game.Id)
                .SelectMany(game => game.Players)
                .ToList();
            return players.All(p => p.Score == game.Words.Count);
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
    }
}
