using Microsoft.AspNetCore.Components;
using PatteDoie.Models.Platform;
using PatteDoie.Models.Scattergories;
using PatteDoie.Rows.Scattegories;
using PatteDoie.Services.Scattergories;

namespace PatteDoie.Views.ScattergoriesGames
{
    public partial class EndGameTest
    {

        public ScattergoriesPlayerRow Winner = default!;

        [Inject]
        protected IScattegoriesService ScattegoriesService { get; set; } = default!;

        [Inject]
        protected PatteDoieContext _context { get; set; } = default!;

        public async Task EndGame()
        {
            List<string> userNames = ["a", "b", "c"];
            var players = new List<User>();
            foreach (var name in userNames)
            {
                players.Add(new User
                {
                    Nickname = name
                });
            }
            var scattergoriesPlayers = new List<ScattergoriesPlayer>();
            foreach (var player in players)
            {
                var scattergoriesPlayer = new ScattergoriesPlayer
                {
                    User = player,
                    Answers = [],
                    IsHost = false,
                    Score = 0,
                };
                _context.ScattergoriesPlayer.Add(scattergoriesPlayer);
                scattergoriesPlayers.Add(scattergoriesPlayer);
            }
            scattergoriesPlayers[0].Score = 5;
            var game = new ScattergoriesGame
            {
                Categories = [],
                CurrentLetter = 'Z',
                CurrentRound = 1,
                MaxRound = 1,
                Players = scattergoriesPlayers,
            };
            _context.ScattergoriesGame.Add(game);


            await _context.SaveChangesAsync();



            Winner = ScattegoriesService.EndScattergoriesGame(game.Id).Result;
        }

    }
}
