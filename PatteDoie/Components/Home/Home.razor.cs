
namespace PatteDoie.Components.Home;

public partial class Home : BasePage
{
    private bool Authenticated = false;

    protected override async Task OnInitializedAsync()
    {
        Authenticated = await IsAuthenticated();
    }
}
