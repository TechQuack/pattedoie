using Microsoft.AspNetCore.Components;
using PatteDoie.Models.Platform;
using PatteDoie.Rows.Scattegories;
using PatteDoie.Services.Scattergories;

namespace PatteDoie.Views.ScattergoriesGames
{
    public partial class CreateGame : ComponentBase
    {
        [Inject]
        protected IScattegoriesService ScattegoriesService { get; set; } = default!;

        private ScattegoriesGameRow? Row { get; set; } = null;

        public void NewGame()
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
            var host = new User
            {
                Nickname = "host"
            };
        }

    }
}
