﻿using AutoMapper;
using Microsoft.AspNetCore.Components;
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

        private readonly PatteDoieContext _context;

        private readonly IMapper _mapper;
        private IHubContext<SpeedTypingHub> _hub;

        private NavigationManager NavigationManager;

        public SpeedTypingService(PatteDoieContext context, IMapper mapper, IHubContext<SpeedTypingHub> hub, NavigationManager navigationManager)
        {
            _context = context;
            _mapper = mapper;
            _hub = hub;
            NavigationManager = navigationManager;
        }

        public async Task<SpeedTypingGameRow> CreateGame(List<User> platformUsers)
        {
            List<SpeedTypingPlayer> players = [];
            foreach (User platformUser in platformUsers)
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
                TimeProgresses = []
            };
            _context.SpeedTypingGame.Add(speedTypingGame);

            await _context.SaveChangesAsync();
            Task endGame = this.DelayedEnding(speedTypingGame);

            return _mapper.Map<SpeedTypingGameRow>(speedTypingGame);
        }

        public async Task DeleteGame(Guid id)
        {
            var game = _context.SpeedTypingGame
                .Include(g => g.Players)
                .Include(g => g.TimeProgresses)
                .FirstOrDefault(g => g.Id == id)
                ?? throw new GameNotValidException("Speed typing game cannot be null");
            _context.SpeedTypingPlayer.RemoveRange(game.Players);
            _context.SpeedTypingTimeProgress.RemoveRange(game.TimeProgresses);
            _context.SpeedTypingGame.Remove(game);
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<SpeedTypingGameRow>> GetAllGames()
        {
            throw new NotImplementedException();
        }

        public async Task<SpeedTypingGameRow> GetGame(Guid gameId)
        {
            var game = await _context.SpeedTypingGame.AsQueryable().Where(g => g.Id == gameId).FirstOrDefaultAsync();
            return _mapper.Map<SpeedTypingGameRow>(game);
        }

        public Task<IEnumerable<SpeedTypingGameRow>> SearchGames(CreateSpeedTypingGameCommand query)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetScore(Guid playerUUID)
        {
            var platformUser = await _context.PlatformUser.AsQueryable().Where(u => u.UserUUID == playerUUID).FirstOrDefaultAsync();
            var player = await _context.SpeedTypingPlayer.AsQueryable().Where(p => p.User == platformUser).FirstOrDefaultAsync()
                ?? throw new PlayerNotValidException("Player not found");
            return player.Score;
        }

        public async Task ManageEndOfGame(Guid gameId)
        {
            var game = _context.SpeedTypingGame.AsQueryable()
               .Where(g => g.Id == gameId)
               .FirstOrDefault<SpeedTypingGame>();
            if (game is null)
            {
                throw new GameNotValidException("Speed typing game cannot be null");
            }
            foreach (SpeedTypingPlayer player in game.Players)
            {
                if (!IsSetTimeProgress(game.TimeProgresses, player))
                {
                    await SetTimeProgress(game, player, DateTime.UtcNow);
                }
            }
            Task deleteGame = this.DelayedDeletion(game);
        }
        public async Task SetTimeProgress(SpeedTypingGame game, SpeedTypingPlayer player, DateTime timeProgress)
        {
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
            game.TimeProgresses.Add(playerProgress);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckWord(Guid gameId, Guid uuid, string word)
        {
            var game = _context.SpeedTypingGame.AsQueryable()
                .Where(g => g.Id == gameId)
                .FirstOrDefault<SpeedTypingGame>() ?? throw new GameNotValidException("Game not found");
            var platformUser = await _context.PlatformUser.AsQueryable().Where(u => u.UserUUID == uuid).FirstOrDefaultAsync();
            var player = await _context.SpeedTypingPlayer.AsQueryable().Where(p => p.User == platformUser).FirstOrDefaultAsync()
                ?? throw new PlayerNotValidException("Player not found");
            var wordIndexToCheck = player.Score;
            if (wordIndexToCheck >= game.Words.Count)
            {
                return false;
            }
            var wordToCheck = game.Words[wordIndexToCheck];
            if (wordToCheck == word)
            {
                player.Score += 1;
                await _context.SaveChangesAsync();
                await _hub.Clients.All.SendAsync("SendProgression", gameId, _mapper.Map<SpeedTypingPlayerRow>(player));
                if (player.Score == game.Words.Count)
                {
                    await SetTimeProgress(game, player, DateTime.UtcNow);
                }
                if (allPlayersHaveFinished(game))
                {
                    await ManageEndOfGame(gameId);
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool allPlayersHaveFinished(SpeedTypingGame game)
        {
            return game.Players.All(p => p.Score == game.Words.Count);
        }

        private bool IsSetTimeProgress(List<SpeedTypingTimeProgress> timeProgresses, SpeedTypingPlayer player)
        {
            return timeProgresses.Any(timeProgress => timeProgress.Player == player);
        }

        private async Task DelayedDeletion(SpeedTypingGame game)
        {
            await Task.Delay(TIME_BEFORE_DELETION);
            await DeleteGame(game.Id);
            NavigationManager.NavigateTo("/lobby");
        }

        private async Task DelayedEnding(SpeedTypingGame game)
        {
            await Task.Delay(TIME_BEFORE_ENDING);
            await ManageEndOfGame(game.Id);
        }
    }
}
