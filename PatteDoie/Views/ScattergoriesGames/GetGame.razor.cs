﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using PatteDoie.Models.Scattergories;
using PatteDoie.Rows.Scattegories;
using PatteDoie.Services.Scattergories;

namespace PatteDoie.Views.ScattergoriesGames
{
    public partial class GetGame : GamePage
    {

        [Parameter]
        public required string Id { get; set; }

        private HubConnection? hubConnection;
        private List<ScattergoriesPlayerRow> _players = [];
        private ScattegoriesGameRow? Row { get; set; } = null;
        private List<ScattergoriesPlayerRow> FinalRanking = [];
        private string UUID;

        [Inject]
        protected IScattegoriesService ScattergoriesService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            this.Row = await ScattergoriesService.GetGame(new Guid(this.Id));
            _players = await ScattergoriesService.GetRank(new Guid(this.Id));
            FinalRanking = _players;
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/hub/scattergories"), (opts) =>
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

            hubConnection.On<ScattergoriesPlayerRow>("ReceiveProgression", async (player) =>
            {
                FinalRanking = await ScattergoriesService.GetRank(new Guid(this.Id));
                await InvokeAsync(StateHasChanged);
            });
            hubConnection.On("RedirectToHome", async (Guid gameId) =>
            {
                if (UUID != null)
                {
                    NavigationManager.NavigateTo("/");
                }
            });

            hubConnection.On("ShowRanking", async (Guid gameId) =>
            {
                FinalRanking = await ScattergoriesService.GetRank(gameId);
                await InvokeAsync(StateHasChanged);

            });

            await hubConnection.StartAsync();
            await hubConnection.SendAsync("JoinGame", this.Id);

            Initialized = true;
        }

        protected override Guid? GetLobbyGuid()
        {
            return Row?.Lobby?.Id;
        }

        public void sendWords(List<string> inputs, List<ScattergoriesCategory> categories)
        {
            for (var i = 0; i < inputs.Count; ++i)
            {
                //TODO call service.addplayerword() avec tous les mots et faire verif
            }
        }
    }
}
