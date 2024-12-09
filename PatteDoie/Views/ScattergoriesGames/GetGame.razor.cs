using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
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
        private string[] inputs = [];
        private bool[] AreWordsCorrect = [];

        [Inject]
        protected IScattegoriesService ScattergoriesService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            this.Row = await ScattergoriesService.GetGame(new Guid(this.Id));

            inputs = new string[Row.Categories.Count];
            AreWordsCorrect = Enumerable.Repeat<bool>(true, Row.Categories.Count).ToArray();
            _players = await ScattergoriesService.GetPlayers(new Guid(this.Id));
            FinalRanking = await ScattergoriesService.GetRank(new Guid(this.Id));
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
                    NavigationManager.NavigateTo("/", forceLoad: true);
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (Initialized)
            {
                await base.OnAfterRenderAsync(firstRender);
                UUID = await GetUUID();
            }
        }

        protected override Guid? GetLobbyGuid()
        {
            return Row?.Lobby?.Id;
        }

        public async void SendWord(int index)
        {
            bool exception = false;
            try
            {
                Row = await ScattergoriesService.AddPlayerWord(new Guid(Id), new Guid(UUID), inputs[index], Row!.Categories[index]);
            }
            catch (Exception ex)
            {
                AreWordsCorrect[index] = false;
                await InvokeAsync(StateHasChanged);
                exception = true;
            }
            if (!exception)
            {
                AreWordsCorrect[index] = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        public async void ConfirmWords()
        {
            Row = await ScattergoriesService.ConfirmWords(new Guid(Id), new Guid(UUID));
            _players = await ScattergoriesService.GetPlayers(new Guid(Id));
            if (Row.IsHostCheckingPhase)
            {
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
