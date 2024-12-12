namespace PatteDoie.Components;

public partial class AuthenticatedPage : BasePage
{
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!await IsAuthenticated())
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
