using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PatteDoie.Rows.SpeedTypingGame;
using PatteDoie.Services.SpeedTyping;

namespace PatteDoie.Views.SpeedTypingGames
{
    public partial class GetGame : ComponentBase
    {
        [BindProperty(SupportsGet = true)]
        [Parameter]
        public required string Id { get; set; }

        private string hasSpace = "No Space detected";

        private SpeedTypingGameRow? Row { get; set; } = null;


        [Inject]
        protected ISpeedTypingService SpeedTypingService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            this.Row = await SpeedTypingService.GetGame(new Guid(this.Id));
        }

        public void CheckTextSpace(string Text)
        {
            if (Text.Contains(' '))
            {
                this.hasSpace = "Space detected";
            }
            else
            {
                this.hasSpace = "No Space detected";
            }
        }
    }
}
