using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using PatteDoie.Rows.Scattegories;
using PatteDoie.Services.Scattergories;

namespace PatteDoie.Views.ScattergoriesGames
{
    public partial class GetGame : ComponentBase
    {

        [Parameter]
        public required string Id { get; set; }

        private HubConnection? HubConnection;
        private ScattegoriesGameRow? Row { get; set; } = null;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        protected IScattegoriesService ScattergoriesService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            this.Row = await ScattergoriesService.GetGame(new Guid(this.Id));

            HubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("/hub/scattergories"), (opts) =>
                {
                    opts.HttpMessageHandlerFactory = (message) =>
                    {
                        if (message is HttpClientHandler clientHandler)
                            clientHandler.ServerCertificateCustomValidationCallback +=
                                (sender, certificate, chain, SslPolicyErrors) => { return true; };
                        return message;
                    };
                })
                .Build();

            await HubConnection.StartAsync();
            await HubConnection.SendAsync("JoinGame", this.Id);
        }
    }
}
