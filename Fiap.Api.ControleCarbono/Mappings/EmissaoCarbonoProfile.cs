using AutoMapper;
using Fiap.Api.ControleCarbono.Models;
using Fiap.Api.ControleCarbono.ViewModel;

namespace Fiap.Api.ControleCarbono.Mappings
{
    public class EmissaoCarbonoProfile : Profile
    {
        public EmissaoCarbonoProfile()
        {
            CreateMap<EmissaoCarbono, EmissaoCarbonoViewModel>().ReverseMap();
        }
    }
}
