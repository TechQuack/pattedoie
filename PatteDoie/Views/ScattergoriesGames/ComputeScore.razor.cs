using Microsoft.AspNetCore.Components;
using PatteDoie.Models.Platform;
using PatteDoie.Models.Scattergories;
using PatteDoie.Services.Scattergories;

namespace PatteDoie.Views.ScattergoriesGames
{
    public partial class ComputeScore
    {
        [Inject]
        protected IScattegoriesService ScattegoriesService { get; set; } = default!;

        [Inject]
        protected PatteDoieContext _context { get; set; } = default!;

        public async Task UpdateScore()
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



            var awnser = new ScattergoriesAnswer
            {
                Category = new ScattergoriesCategory
                {
                    Name = "test"
                },
                IsChecked = false,
                Text = "test",
            };
            _context.ScattegoriesAnswer.Add(awnser);

            await _context.SaveChangesAsync();

            await ScattegoriesService.HostVerifyWord(game.Id, game.Players.First(), awnser, true);

            if (game.Players.First().Score == 1)
            {
                Console.WriteLine("c'est good");
            }
        }
    }
}
