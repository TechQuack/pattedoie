using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;


namespace PatteDoie.Views.Home;

public partial class Authenticator : ComponentBase
{

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private ProtectedLocalStorage ProtectedLocalStorage { get; set; } = default!;

    private string Name { get; set; } = string.Empty;


    private async Task RegisterUser()
    {
        await ProtectedLocalStorage.SetAsync("name", Name);
        await GenerateUUID();
        NavigationManager.NavigateTo("/lobby");
    }

    private async Task GenerateUUID()
    {
        var uuid = Guid.NewGuid().ToString();
        await ProtectedLocalStorage.SetAsync("uuid", uuid);
    }
}
