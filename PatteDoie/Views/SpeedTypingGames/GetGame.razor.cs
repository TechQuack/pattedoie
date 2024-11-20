﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PatteDoie.Rows.SpeedTypingGame;
using PatteDoie.Services.SpeedTyping;

namespace PatteDoie.Views.SpeedTypingGames
{
    public partial class GetGame : ComponentBase
    {
        [BindProperty(SupportsGet = true)]
        [Parameter]
        public required string Id { get; set; }

        private string hasSpace = "No Space detected";

        private bool result = false;

        private SpeedTypingGameRow? Row { get; set; } = null;


        [Inject]
        protected ISpeedTypingService SpeedTypingService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            this.Row = await SpeedTypingService.GetGame(new Guid(this.Id));
        }

        public void CheckTextSpace(string Text)
        {
            if (Text.Contains(' '))
            {
                this.hasSpace = "Space detected";
                if (Task.Run(() => this.SpeedTypingService.CheckWord(this.Row.Id, Guid.Parse("26D68432-14A9-4E12-9564-DAD5F9EDF644"), Text.TrimEnd())).Result)
                {
                    this.result = true;
                }
                else
                {
                    this.result = false;
                }

            }
            else
            {
                this.hasSpace = "No Space detected";
            }
        }
    }
}
