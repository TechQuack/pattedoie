using Microsoft.AspNetCore.Components;
using PatteDoie.Enums;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views.Platform;

public partial class CreateLobby : AuthenticatedPage
{
    [Parameter, SupplyParameterFromQuery]
    public string? GameName { get; set; }
    private string? Password { get; set; }
    private string LobbyName { get; set; } = "";

    [Inject]
    private IPlatformService PlatformService { get; set; } = default!;

    private async Task Submit()
    {
        var uuid = await GetUUID();
        var name = await GetName();

        GameType gameType;
        try
        {
            gameType = GameTypeHelper.GetGameTypeFromString(GameName ?? "");
        }
        catch
        {
            //TODO: Notify error invalid game
            return;
        }

        var lobbyRow = await PlatformService.CreateLobby(new Guid(uuid), name, Password, gameType, LobbyName);

        NavigationManager.NavigateTo($"/lobby/{lobbyRow.Id}");
    }
}