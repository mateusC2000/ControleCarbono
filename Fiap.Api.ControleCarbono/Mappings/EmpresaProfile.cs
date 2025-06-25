using AutoMapper;
using Fiap.Api.ControleCarbono.Models;
using Fiap.Api.ControleCarbono.ViewModel;

namespace Fiap.Api.ControleCarbono.Mappings
{
    public class EmpresaProfile : Profile
    {
        public EmpresaProfile()
        {
            CreateMap<Empresa, EmpresaViewModel>().ReverseMap();
        }
    }
}
