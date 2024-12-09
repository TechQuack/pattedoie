using Microsoft.AspNetCore.Components;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views;

public abstract partial class GamePage : BasePage
{

    [Inject]
    protected IPlatformService PlatformService { get; set; } = default!;

    protected bool Initialized = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Initialized)
        {
            var id = GetLobbyGuid();
            if (id == null) { return; }
            var lobby = await PlatformService.GetLobby(id ?? new Guid());
            if (lobby == null)
            {
                NavigationManager.NavigateTo("/", forceLoad: true);
                return;
            }
            var uuid = await GetUUID();
            if (!Guid.TryParse(uuid, out Guid guid))
            {
                NavigationManager.NavigateTo("/lobby", forceLoad: true);
                return;
            }
            var user = lobby.Users.Find(u => u.UserUUID == guid);
            if (user == null)
            {
                NavigationManager.NavigateTo("/lobby", forceLoad: true);
                return;
            }
        }
    }

    protected abstract Guid? GetLobbyGuid();
}
