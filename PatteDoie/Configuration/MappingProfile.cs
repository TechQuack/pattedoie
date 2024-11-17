using AutoMapper;
using PatteDoie.Models.SpeedTyping;
using PatteDoie.Rows.SpeedTypingGame;

namespace PatteDoie.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SpeedTypingGame, SpeedTypingGameRow>();
        }
    }
}
