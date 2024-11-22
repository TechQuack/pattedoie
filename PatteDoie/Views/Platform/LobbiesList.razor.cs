using Microsoft.AspNetCore.Components;
using PatteDoie.Rows.Platform;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views.Platform;

public partial class LobbiesList : ComponentBase
{
    public IEnumerable<PlatformLobbyRow> Items = [];

    [Inject]
    private IPlatformService PlatformService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Items = await PlatformService.GetPublicLobbies();
    }

}