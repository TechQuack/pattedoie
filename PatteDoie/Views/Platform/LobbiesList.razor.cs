using Microsoft.AspNetCore.Components;
using PatteDoie.Enums;
using PatteDoie.Extensions;
using PatteDoie.Rows.Platform;
using PatteDoie.Services;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views.Platform;

public partial class LobbiesList : AuthenticatedPage
{
    public IEnumerable<PlatformLobbyRow> Items = [];

    public Dictionary<PlatformLobbyRow, Guid?> GameUUIDFromLobbies = [];

    [Inject]
    private IPlatformService PlatformService { get; set; } = default!;

    private LobbyType Type = LobbyType.All;

    private FilterGameType GameTypeValue = FilterGameType.All;

    private Dictionary<PlatformLobbyRow, bool> IsUserInLobbies = [];

    private readonly List<LobbyType> Types =
    [
        LobbyType.Public,
        LobbyType.Private,
        LobbyType.All
    ];

    [Inject]
    private IClipboardService ClipboardService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await SearchLobbies();
        foreach (var item in Items)
        {
            if (item.Started)
            {
                GameUUIDFromLobbies.Add(item, await PlatformService.GetGameUUIDFromLobby(item.Id));
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            var uuid = await GetUUID();
            Guid.TryParse(uuid, out Guid userId);
            foreach (var item in Items)
            {
                if (item.Started)
                {
                    IsUserInLobbies[item] = item.Users.Exists(u => u.UserUUID == userId);
                }
            }
            await InvokeAsync(StateHasChanged);
        }
    }

    protected async void CopyLink(Guid lobbyId)
    {
        await ClipboardService.CopyLobbyLink(lobbyId);
    }
    private async void JoinPublicLobby(Guid Id)
    {
        var uuid = await GetUUID();
        var name = await GetName();


        try
        {
            await PlatformService.JoinLobby(Id, name, new Guid(uuid), "");
        }
        catch (Exception ex)
        {
            ToastService.Notify(new(BlazorBootstrap.ToastType.Danger, "Error joining lobby", ex.Message));
            return;
        }
        ToastService.Notify(new(BlazorBootstrap.ToastType.Success, "Success", "Lobby joined"));
        NavigationManager.NavigateTo($"/lobby/{Id}");
    }

    private static string GetLobbyTypeDescription(LobbyType type)
    {
        return type.GetDescription();
    }

    private async Task SearchLobbies()
    {
        Items = await PlatformService.SearchLobbies(Type, GameTypeValue);
    }
}