using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using PatteDoie.Enums;
using PatteDoie.Rows.Platform;
using PatteDoie.Services;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views.Platform;

public partial class LobbyDetail : AuthenticatedPage
{
    [Parameter]
    public string Id { get; set; } = string.Empty;
    private string UUID;
    private bool IsCreator = false;
    private PlatformLobbyRow? Lobby { get; set; } = null;

    private HubConnection? hubConnection;



    [Inject]
    private IPlatformService PlatformService { get; set; } = default!;

    [Inject]
    private IClipboardService ClipboardService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var guid = new Guid(Id);
        Lobby = await PlatformService.GetLobby(guid);

        hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/hub/platform"), (opts) =>
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

        hubConnection.On<Guid>("ReceiveGameStarted", RedirectToGame);
        hubConnection.On<Guid>("ReceivePlayerJoined", async (id) =>
        {
            var user = await PlatformService.GetUser(id, Lobby.Id);
            if (user != null)
            {
                Lobby.Users.Add(user);
                await InvokeAsync(StateHasChanged);
            }
        });
        await hubConnection.StartAsync();
        await hubConnection.SendAsync("JoinLobby", this.Id);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            UUID = await GetUUID();
            IsCreator = await IsHost(new Guid(UUID));
        }
    }

    protected async Task StartGame()
    {
        if (Lobby != null)
        {
            await PlatformService.StartGame(Lobby.Id, new Guid(UUID));

        }
    }

    protected async void CopyLink()
    {
        if (Lobby != null)
        {
            await ClipboardService.CopyLobbyLink(Lobby.Id);
        }
    }

    private async Task<Boolean> IsHost(Guid uuid)
    {
        return await PlatformService.IsHost(uuid, Lobby.Creator.UserUUID, Lobby.Id);
    }
    private async Task RedirectToGame(Guid gameId)
    {
        if (Lobby == null) { return; }
        var gameType = GameTypeHelper.GetGameTypeFromString(Lobby.Game.Name);
        switch (gameType)
        {
            case GameType.SpeedTyping:
                NavigationManager.NavigateTo($"/speedtyping/{gameId}");
                break;
            case GameType.Scattergories:
                NavigationManager.NavigateTo($"/scattergories/{gameId}");
                break;
        }
    }
}