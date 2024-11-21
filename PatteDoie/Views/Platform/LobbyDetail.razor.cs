using Microsoft.AspNetCore.Components;
using PatteDoie.Rows.Platform;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views.Platform;

public partial class LobbyDetail : ComponentBase
{
    [Parameter]
    public string Id { get; set; } = string.Empty;

    private PlatformLobbyRow? Lobby { get; set; } = null;

    [Inject]
    private IPlatformService PlatformService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var guid = new Guid(Id);
        Lobby = await PlatformService.GetLobby(guid);
    }
}