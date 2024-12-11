using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views.Platform
{
    public partial class LobbyJoin : AuthenticatedPage
    {
        [BindProperty(SupportsGet = true)]
        [Parameter]
        public required string Id { get; set; }
        private string? Password { get; set; }

        [Inject]
        private IPlatformService PlatformService { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (await IsPublic())
            {
                Submit();
            }
        }

        private async void Submit()
        {
            var uuid = await GetUUID();
            var name = await GetName();

            try
            {
                await PlatformService.JoinLobby(new Guid(Id ?? ""), name, new Guid(uuid), Password);
            }
            catch (Exception ex)
            {
                // TODO : display an error to the user
            }

            NavigationManager.NavigateTo($"/lobby/{Id}");
        }

        private async Task<Boolean> IsPublic()
        {
            var lobby = await PlatformService.GetLobby(new Guid(Id ?? ""));
            return lobby?.Password?.Trim().IsNullOrEmpty() ?? true;
        }

    }
}
