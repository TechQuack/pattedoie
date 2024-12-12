using AutoMapper;
using PatteDoie.Models.Platform;
using PatteDoie.Models.Scattergories;
using PatteDoie.Models.SpeedTyping;
using PatteDoie.Rows.Platform;
using PatteDoie.Rows.Scattegories;
using PatteDoie.Rows.SpeedTyping;
using PatteDoie.Rows.SpeedTypingGame;

namespace PatteDoie.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SpeedTypingGame, SpeedTypingGameRow>();
            CreateMap<SpeedTypingPlayer, SpeedTypingPlayerRow>();
            CreateMap<SpeedTypingTimeProgress, SpeedTypingTimeProgressRow>();
            CreateMap<HighScore, PlatformHighScoreRow>();
            CreateMap<ScattergoriesGame, ScattergoriesGameRow>();
            CreateMap<ScattergoriesPlayer, ScattergoriesPlayerRow>();
            CreateMap<ScattergoriesCategory, ScattergoriesCategoryRow>();
            CreateMap<User, PlatformUserRow>();
            CreateMap<Lobby, PlatformLobbyRow>();
            CreateMap<Game, GameRow>();
        }
    }
}
