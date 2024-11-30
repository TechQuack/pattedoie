﻿using Microsoft.AspNetCore.Components;
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

        private Timer _timer = null!;
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
        private IJSRuntime JSRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.Row = await SpeedTypingService.GetGame(new Guid(this.Id));
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

            hubConnection.On<SpeedTypingPlayerRow>("ReceiveProgress", (player) =>
            {
                //TODO : update front
                InvokeAsync(StateHasChanged);
            });

            await hubConnection.StartAsync();
            await hubConnection.SendAsync("JoinGame", this.Id);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var uuid = await ProtectedLocalStorage.GetAsync<string>("uuid");

                var elapsedTime = DateTime.UtcNow - Row.LaunchTime;
                _secondsToRun = 60 - (int)elapsedTime.TotalSeconds;

                WordIndexToDisplay = await SpeedTypingService.GetScore(new Guid(uuid.Value ?? ""));
            }
        }

        public async void CheckTextSpace(string Text)
        {
            if (Text.Contains(' '))
            {
                var uuid = await ProtectedLocalStorage.GetAsync<string>("uuid");

                if (Task.Run(() => this.SpeedTypingService.CheckWord(this.Row.Id, new Guid(uuid.Value ?? ""), Text.TrimEnd())).Result)
                {
                    this.WordIndexToDisplay += 1;
                    await JSRuntime.InvokeVoidAsync("eval", $"document.getElementById('inputText').value = ''");
                }


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
    }
}
