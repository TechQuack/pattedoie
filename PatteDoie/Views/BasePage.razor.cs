using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace PatteDoie.Views;

public partial class BasePage : ComponentBase
{
    [Inject]
    protected ProtectedLocalStorage ProtectedLocalStorage { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    protected ToastService ToastService { get; set; } = default!;

    protected async Task<bool> IsAuthenticated()
    {
        try
        {
            var uuid = await ProtectedLocalStorage.GetAsync<string>("uuid");
            var name = await ProtectedLocalStorage.GetAsync<string>("name");

            return (uuid.Value ?? "") != "" && (name.Value ?? "") != "";
        }
        catch
        {
            return false;
        }
    }
    protected async Task<string> GetUUID()
    {
        var result = await ProtectedLocalStorage.GetAsync<string>("uuid");
        if (!result.Success)
        {
            return "";
        }
        return result.Value ?? "";
    }

    protected async Task<string> GetName()
    {
        var result = await ProtectedLocalStorage.GetAsync<string>("name");
        if (!result.Success)
        {
            return "";
        }
        return result.Value ?? "";
    }
}
