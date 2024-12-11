using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using PatteDoie.Enums;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views.Platform;

public partial class CreateLobby : AuthenticatedPage
{
    [Parameter, SupplyParameterFromQuery]
    public string? GameName { get; set; }
    private string Password { get; set; } = "";
    private string LobbyName { get; set; } = "";

    private bool IsLobbyPublic { get; set; } = true;

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
            ToastService.Notify(new(BlazorBootstrap.ToastType.Danger, "Error creating lobby", "Game doest not exist"));
            return;
        }
        if (!IsLobbyPublic && Password.Trim().IsNullOrEmpty())
        {
            ToastService.Notify(new(BlazorBootstrap.ToastType.Danger, "Error creating lobby", "Lobby password is invalid"));
            return;
        }
        if (LobbyName.Trim().IsNullOrEmpty())
        {
            ToastService.Notify(new(BlazorBootstrap.ToastType.Danger, "Error creating lobby", "Lobby needs a valid name"));
            return;
        }
        var lobbyRow = await PlatformService.CreateLobby(new Guid(uuid), name, IsLobbyPublic ? "" : Password, gameType, LobbyName);
        ToastService.Notify(new(BlazorBootstrap.ToastType.Success, "Success", "Lobby created successfully"));
        NavigationManager.NavigateTo($"/lobby/{lobbyRow.Id}");
    }
}