namespace PatteDoie.Services
{
    public interface IClipboardService
    {
        public Task CopyLobbyLink(Guid lobbyId);
    }
}
