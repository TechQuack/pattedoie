using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views;

public abstract partial class GamePage : BasePage
{

    [Inject]
    protected IPlatformService PlatformService { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var lobby = await PlatformService.GetLobby(GetLobbyGuid());
        if (lobby == null)
        {
            NavigationManager.NavigateTo("/");
            return;
        }
        var uuid =await GetUUID();
        if (!Guid.TryParse(uuid, out Guid guid))
        {
            NavigationManager.NavigateTo("/lobby");
            return;
        }
        var user = lobby.Users.Find(u => u.UserUUID == guid);
        if (user == null)
        {
            NavigationManager.NavigateTo("/lobby");
            return;
        }
    }

    protected abstract Guid GetLobbyGuid();
}
