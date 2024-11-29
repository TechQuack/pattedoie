namespace PatteDoie.Views;

public partial class AuthenticatedPage : BasePage
{
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!await IsAuthenticated())
        {
            NavigationManager.NavigateTo("/");
        }
    }

    protected async Task<string> GetUUID()
    {
        return (await ProtectedLocalStorage.GetAsync<string>("uuid")).Value ?? "";
    }

    protected async Task<string> GetName()
    {
        return (await ProtectedLocalStorage.GetAsync<string>("name")).Value ?? "";
    }
}
