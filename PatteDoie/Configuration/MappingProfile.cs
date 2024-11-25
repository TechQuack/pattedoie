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
            CreateMap<HighScore, PlatformHighScoreRow>();
            CreateMap<User, PlatformUserRow>();
            CreateMap<Lobby, PlatformLobbyRow>();
            CreateMap<Game, GameRow>();
        }
    }
}
