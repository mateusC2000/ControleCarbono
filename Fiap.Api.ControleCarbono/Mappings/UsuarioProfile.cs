using AutoMapper;
using Fiap.Api.ControleCarbono.Models;
using Fiap.Api.ControleCarbono.ViewModel;

namespace Fiap.Api.ControleCarbono.Mappings
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<UsuarioViewModel, Usuario>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}