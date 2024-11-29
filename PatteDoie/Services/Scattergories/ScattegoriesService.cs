﻿using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PatteDoie.Models.Platform;
using PatteDoie.Models.Scattergories;
using PatteDoie.PatteDoieException;
using PatteDoie.Rows.Platform;
using PatteDoie.Rows.Scattegories;

namespace PatteDoie.Services.Scattergories
{
    public class ScattegoriesService(PatteDoieContext context, IMapper mapper) : IScattegoriesService
    {
        public static int TIME_BEFORE_DELETION = 60000;

        private readonly PatteDoieContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly NavigationManager NavigationManager = default!;

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

        public async Task<bool> AddPlayerWord(ScattergoriesGame game, ScattergoriesPlayer player, string word, ScattergoriesCategory category)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            if (word.Trim().IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(word));
            }
            char letter = game.CurrentLetter;
            if (word.Trim().First().Equals(letter))
            {
                return false;
            }
            ScattergoriesAnswer answer = new ScattergoriesAnswer
            {
                Category = category,
                Text = word
            };
            player.Answers.Add(answer);
            /*
             * TODO: check if user completed categories: if yes call checking by host player
             */
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ScattegoriesGameRow> CreateGame(int numberCategories, int roundNumber, List<User> users, User host)
        {
            var rand = new Random();

            var potentialsCategories = (await _context.ScattergoriesCategory.AsQueryable().ToListAsync());
            List<ScattergoriesCategory> categories = potentialsCategories.OrderBy(x => rand.Next()).Take(numberCategories).ToList();

            var players = new List<ScattergoriesPlayer>();
            foreach (var user in users)
            {
                var playerAnswers = new List<ScattergoriesAnswer>();
                var player = CreatePlayer(user, playerAnswers, false);
                players.Add(player);
                _context.ScattergoriesPlayer.Add(player);
            }
            var hostAnswers = new List<ScattergoriesAnswer>();
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

        public async Task DeleteGame(Guid gameId)
        {
            var game = _context.ScattergoriesGame.AsQueryable()
               .Where(g => g.Id == gameId)
               .FirstOrDefault<ScattergoriesGame>() ?? throw new GameNotValidException("Scattergories game cannot be null");
            _context.ScattergoriesPlayer.RemoveRange(game.Players);
            _context.ScattergoriesCategory.RemoveRange(game.Categories);
            _context.ScattergoriesGame.Remove(game);
            await _context.SaveChangesAsync();
        }

        public async Task<PlatformUserRow> EndScattergoriesGame(Guid gameId)
        {
            var game = _context.ScattergoriesGame.AsQueryable().Where(g => g.Id == gameId).FirstOrDefault<ScattergoriesGame>()
                    ?? throw new Exception("Scattergories game is null");
            if (!IsGameEnded(game))
            {
                throw new Exception("Scattergories game is not ended");
            }
            var players = game.Players;
            ScattergoriesPlayer bestPlayer = players.First();
            foreach (var player in players)
            {
                if (player.Score > bestPlayer.Score)
                {
                    bestPlayer = player;
                }
            }
            var bestUser = bestPlayer.User;

            Task deleteGame = this.DelayedDeletion(game);

            return _mapper.Map<PlatformUserRow>(bestUser);
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

        private static bool IsGameEnded(ScattergoriesGame game)
        {
            if (game.CurrentRound == game.MaxRound)
            {
                return true;
            }
            return false;
        }

        private async Task DelayedDeletion(ScattergoriesGame game)
        {
            await Task.Delay(TIME_BEFORE_DELETION);
            await DeleteGame(game.Id);
            NavigationManager.NavigateTo("/home");
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
    }
}
