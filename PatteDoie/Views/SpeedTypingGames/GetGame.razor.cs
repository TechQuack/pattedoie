using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
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
        private int _secondsToRun = 60;

        private string HasSpace = "No Space detected";

        private bool Result = false;

        private int WordIndexToDisplay = 0;

        private SpeedTypingGameRow? Row { get; set; } = null;
        [Inject]
        private ProtectedLocalStorage ProtectedLocalStorage { get; set; } = default!;


        [Inject]
        protected ISpeedTypingService SpeedTypingService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            this.Row = await SpeedTypingService.GetGame(new Guid(this.Id));
        }

        public async void EndGame()
        {
            await this.SpeedTypingService.ManageEndOfGame(this.Row.Id);
        }

        public async void CheckTextSpace(string Text)
        {
            if (Text.Contains(' '))
            {
                this.HasSpace = "Space detected";
                var uuid = await ProtectedLocalStorage.GetAsync<string>("uuid");

                if (Task.Run(() => this.SpeedTypingService.CheckWord(this.Row.Id, new Guid(uuid.Value ?? ""), Text.TrimEnd())).Result)
                {
                    this.WordIndexToDisplay += 1;
                    this.Result = true;
                }
                else
                {
                    this.Result = false;
                }

            }
            else
            {
                this.HasSpace = "No Space detected";
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
            _secondsToRun = _secondsToRun > 0 ? _secondsToRun - 1 : _secondsToRun;
            await InvokeAsync(StateHasChanged);
        }
    }
}
