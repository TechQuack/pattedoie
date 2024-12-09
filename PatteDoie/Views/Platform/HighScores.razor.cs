using Microsoft.AspNetCore.Components;
using PatteDoie.Rows.Platform;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views.Platform
{
    public partial class HighScores : AuthenticatedPage
    {
        private List<PlatformHighScoreRow> SpeedTypingHighScores = [];
        private List<PlatformHighScoreRow> ScattergoriesHighScores = [];

        [Inject]
        protected IPlatformService PlatformService { get; set; } = default!;

        protected async override Task OnInitializedAsync()
        {
            this.SpeedTypingHighScores = await PlatformService.GetGameHighScores("SpeedTyping");
            this.ScattergoriesHighScores = await PlatformService.GetGameHighScores("Scattergories");
        }
    }
}
