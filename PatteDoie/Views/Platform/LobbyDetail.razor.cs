using Microsoft.AspNetCore.Components;
using PatteDoie.Enums;
using PatteDoie.Extensions;
using PatteDoie.Rows.Platform;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views.Platform;

public partial class LobbyDetail : AuthenticatedPage
{
    [Parameter]
    public string Id { get; set; } = string.Empty;

    private PlatformLobbyRow? Lobby { get; set; } = null;

    [Inject]
    private IPlatformService PlatformService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var guid = new Guid(Id);
        Lobby = await PlatformService.GetLobby(guid);
    }

    protected async Task StartGame()
    {
        if (Lobby != null)
        {
            var id = await PlatformService.StartGame(Lobby.Id);
            var gameType = GameTypeHelper.GetGameTypeFromString(Lobby.Game.Name);
            switch (gameType)
            {
                case GameType.SpeedTyping:
                    NavigationManager.NavigateTo($"/speedtyping/{id}");
                    break;
                case GameType.Scattergories:
                    NavigationManager.NavigateTo($"/scattergories/{id}");
                    break;
            }
        }
    }
}