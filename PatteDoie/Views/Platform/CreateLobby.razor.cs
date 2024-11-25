using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views.Platform;

public partial class CreateLobby : ComponentBase
{
    private string? Password { get; set; }

    [Inject]
    private ProtectedLocalStorage ProtectedLocalStorage { get; set; } = default!;

    [Inject]
    private IPlatformService PlatformService { get; set; } = default!;

    private async Task Submit()
    {
        var uuid = await ProtectedLocalStorage.GetAsync<string>("uuid");
        var name = await ProtectedLocalStorage.GetAsync<string>("name");

        await PlatformService.CreateLobby(new Guid(uuid.Value ?? ""), name.Value ?? "", Password);
    }
}