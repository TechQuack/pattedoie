using Microsoft.AspNetCore.Components;
using PatteDoie.Models.SpeedTyping;
using PatteDoie.Rows.SpeedTypingGame;
using PatteDoie.Services.SpeedTyping;

namespace PatteDoie.Views.SpeedTypingGames
{
    public partial class GetGame : ComponentBase
    {
        public SpeedTypingGameRow GameRow;

        public SpeedTypingService Service;

        public SpeedTypingGame Model;

        public void CheckTextSpace(string Text)
        {
            if (Text.Contains(' '))
            {
                Console.WriteLine("success");
            }
        }
    }
}
