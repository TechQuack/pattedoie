using Microsoft.AspNetCore.Components;
using PatteDoie.Enums;
using PatteDoie.Extensions;
using PatteDoie.Rows.Platform;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views.Platform;

public partial class LobbiesList : AuthenticatedPage
{
    public IEnumerable<PlatformLobbyRow> Items = [];

    [Inject]
    private IPlatformService PlatformService { get; set; } = default!;

    private LobbyType Type = LobbyType.Public;

    private FilterGameType GameType = FilterGameType.All;

    private readonly List<LobbyType> Types =
    [
        LobbyType.Public,
        LobbyType.Private,
        LobbyType.All
    ];

    protected override async Task OnInitializedAsync()
    {
        await SearchLobbies();
    }

    private static string GetLobbyTypeDescription(LobbyType type)
    {
        return type.GetDescription();
    }

    private async Task SearchLobbies()
    {
        Items = await PlatformService.SearchLobbies(Type, GameType);
    }
}