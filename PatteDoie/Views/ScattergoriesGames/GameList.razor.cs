using Microsoft.AspNetCore.Components;
using PatteDoie.Rows.Scattegories;
using PatteDoie.Services.Scattergories;

namespace PatteDoie.Views.ScattergoriesGames
{
    public partial class GameList : ComponentBase
    {
        public IEnumerable<ScattergoriesGameRow> Items = [];

        [Inject]
        private IScattegoriesService ScattergoriesService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await SearchGames();
        }

        private async Task SearchGames()
        {
            Items = await ScattergoriesService.GetAllGames();
        }
    }
}
