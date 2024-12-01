using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using PatteDoie.Rows.SpeedTyping;
using PatteDoie.Rows.SpeedTypingGame;
using PatteDoie.Services.SpeedTyping;
using System.Timers;
using Timer = System.Timers.Timer;
namespace PatteDoie.Views.SpeedTypingGames
{
    public partial class GetGame : ComponentBase
    {
        [BindProperty(SupportsGet = true)]
        [Parameter]
        public required string Id { get; set; }

        private List<SpeedTypingPlayerRow> _players = [];

        private Timer _timer = null!;
        private Timer _timerToDisableInput = null!;
        private int _secondsToRun = 0;
        private HubConnection? hubConnection;

        private ElementReference InputTextRef;
        private int WordIndexToDisplay = 0;

        private SpeedTypingGameRow? Row { get; set; } = null;

        [Inject]
        private ProtectedLocalStorage ProtectedLocalStorage { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        protected ISpeedTypingService SpeedTypingService { get; set; } = default!;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        private ProtectedBrowserStorageResult<string> uuid;

        private bool MustCheckDisabled = true;

        protected override async Task OnInitializedAsync()
        {
            this.Row = await SpeedTypingService.GetGame(new Guid(this.Id));
            _players = await SpeedTypingService.GetRank(new Guid(this.Id));
            hubConnection = new HubConnectionBuilder()
             .WithUrl(Navigation.ToAbsoluteUri("/hub/speedtyping"), (opts) =>
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
                uuid = await ProtectedLocalStorage.GetAsync<string>("uuid");

                var elapsedTime = DateTime.UtcNow - Row.LaunchTime;
                _secondsToRun = 60 - (int)elapsedTime.TotalSeconds;

                WordIndexToDisplay = await SpeedTypingService.GetScore(new Guid(uuid.Value ?? ""));

                _timerToDisableInput = new Timer(1000);
                _timerToDisableInput.Elapsed += CanPlay;
                _timerToDisableInput.AutoReset = true;
                _timerToDisableInput.Enabled = true;
            }
        }

        public async void CheckTextSpace(string Text)
        {
            if (Text.Contains(' '))
            {
                if (Task.Run(() => this.SpeedTypingService.CheckWord(this.Row.Id, new Guid(uuid.Value ?? ""), Text.TrimEnd())).Result)
                {
                    this.WordIndexToDisplay += 1;
                    await JSRuntime.InvokeVoidAsync("eval", $"document.getElementById('inputText').value = ''");
                }


            }
        }

        public async ValueTask DisposeAsync()
        {
            MustCheckDisabled = false;
            if (_timer != null)
            {
                _timer.Dispose();
            }

            if (_timerToDisableInput != null)
            {
                _timerToDisableInput.Dispose();
            }

            if (hubConnection != null)
            {
                await hubConnection.DisposeAsync();
            }
        }

        override
        protected void OnInitialized()
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

        private async void CanPlay(object? sender, ElapsedEventArgs e)
        {
            if (!MustCheckDisabled)
            {
                return;
            }

            try
            {
                if (!await SpeedTypingService.CanPlay(new Guid(uuid.Value ?? ""), Row.Id))
                {
                    await InvokeAsync(async () =>
                    {
                        if (MustCheckDisabled)
                        {
                            await JSRuntime.InvokeVoidAsync("eval", $"document.getElementById('inputText').disabled = 'disabled'");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                await DisposeAsync();
            }
        }
    }
}
