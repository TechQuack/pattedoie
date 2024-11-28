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

        public async Task<ScattegoriesGameRow> NextRound(ScattergoriesGame game)
        {
            if (HasGameEnded(game))
            {
                return EndScattergoriesGame(game).Result;
            } else
            {
                game.CurrentLetter = RandomLetter();
                game.CurrentRound += 1;
                foreach (var player in game.Players) 
                {
                    player.Answers = new List<ScattegoriesAnswer>();
                }
                await _context.SaveChangesAsync();
                return _mapper.Map<ScattegoriesGameRow>(game);
            }
        }

        public async Task<ScattegoriesGameRow> CreateGame(int numberCategories, int roundNumber, List<User> users, User host)
        {
            var rand = new Random();

            var potentialsCategories = (await _context.ScattergoriesCategory.AsQueryable().ToListAsync());
            List<ScattergoriesCategory> categories = potentialsCategories.OrderBy(x => rand.Next()).Take(numberCategories).ToList();

            var players = new List<ScattergoriesPlayer>();
            foreach (var user in users)
            {
                var playerAnswers = new List<ScattegoriesAnswer>();
                var player = CreatePlayer(user, playerAnswers, false);
                players.Add(player);
                _context.ScattergoriesPlayer.Add(player);
            }
            var hostAnswers = new List<ScattegoriesAnswer>();
            var hostPlayer = CreatePlayer(host, hostAnswers, true);
            players.Add(hostPlayer);
            _context.ScattergoriesPlayer.Add(hostPlayer);

            var game = new ScattergoriesGame
            {
                Players = players,
                MaxRound = roundNumber,
                CurrentRound = 1,
                CurrentLetter = RandomLetter(),
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
        
        public async Task<ScattegoriesGameRow> EndScattergoriesGame(ScattergoriesGame game)
        {
            if (!HasGameEnded(game))
            {
                throw new Exception("Scattergories game is not ended");
            }

            Task deleteGame = this.DelayedDeletion(game);

            return _mapper.Map<ScattegoriesGameRow>(game);
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
            return (char) new Random().Next(65, 90);
        }
    }
}
