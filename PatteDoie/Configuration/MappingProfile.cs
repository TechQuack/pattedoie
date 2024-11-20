using AutoMapper;
using PatteDoie.Models.Platform;
using PatteDoie.Models.SpeedTyping;
using PatteDoie.Rows.Platform;
using PatteDoie.Rows.SpeedTypingGame;

namespace PatteDoie.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SpeedTypingGame, SpeedTypingGameRow>();
            CreateMap<PlatformUser, PlatformUserRow>();
            CreateMap<PlatformLobby, PlatformLobbyRow>();
        }
    }
}
