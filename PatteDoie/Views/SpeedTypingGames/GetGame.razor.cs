using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using PatteDoie.Rows.SpeedTyping;
using PatteDoie.Rows.SpeedTypingGame;
using PatteDoie.Services.SpeedTyping;
using System.Timers;
using Timer = System.Timers.Timer;

namespace PatteDoie.Views.SpeedTypingGames
{
    public partial class GetGame : BasePage
    {
        [BindProperty(SupportsGet = true)]
        [Parameter]
        public required string Id { get; set; }

        private List<SpeedTypingPlayerRow> _players = [];
        private Timer _timer = null!;
        private int _secondsToRun = 0;
        private HubConnection? hubConnection;
        private int WordIndexToDisplay = 0;
        private string? inputValue;

        private SpeedTypingGameRow? Row { get; set; } = null;

        [Inject]
        protected ISpeedTypingService SpeedTypingService { get; set; } = default!;


        protected override async Task OnInitializedAsync()
        {
            this.Row = await SpeedTypingService.GetGame(new Guid(this.Id));
            _players = await SpeedTypingService.GetRank(new Guid(this.Id));
            hubConnection = new HubConnectionBuilder()
             .WithUrl(NavigationManager.ToAbsoluteUri("/hub/speedtyping"), (opts) =>
             {
                 opts.HttpMessageHandlerFactory = (message) =>
                 {
                     if (message is HttpClientHandler clientHandler)
                         // always verify the SSL certificate
                         clientHandler.ServerCertificateCustomValidationCallback +=
                             (sender, certificate, chain, sslPolicyErrors) => { return true; };
                     return message;
                 };
             })
             .Build();

            hubConnection.On<SpeedTypingPlayerRow>("ReceiveProgression", async (player) =>
            {
                _players = await SpeedTypingService.GetRank(new Guid(this.Id));
                await InvokeAsync(StateHasChanged);
            });

            await hubConnection.StartAsync();
            await hubConnection.SendAsync("JoinGame", this.Id);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var uuid = await GetUUID();

                var elapsedTime = DateTime.UtcNow - Row.LaunchTime;
                _secondsToRun = 60 - (int)elapsedTime.TotalSeconds;

                WordIndexToDisplay = await SpeedTypingService.GetScore(new Guid(uuid));
            }
        }

        public async void CheckTextSpace(string Text)
        {
            inputValue = Text;
            await InvokeAsync(StateHasChanged);
            if (!Text.Contains(' '))
            {
                return;
            }
            var uuid = await GetUUID();

            if (Task.Run(() => this.SpeedTypingService.CheckWord(this.Row!.Id, new Guid(uuid), Text.TrimEnd())).Result)
            {
                this.WordIndexToDisplay += 1;
                inputValue = "";
                await InvokeAsync(StateHasChanged);
            }

        }

        protected override void OnInitialized()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private async void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            _secondsToRun = _secondsToRun > 0 ? _secondsToRun - 1 : 0;
            await InvokeAsync(StateHasChanged);
        }
    }
}
