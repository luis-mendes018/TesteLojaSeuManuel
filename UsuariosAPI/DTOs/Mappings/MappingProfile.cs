using AutoMapper;

using UsuariosAPI.Models;

namespace UsuariosAPI.DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, UsuarioDTO>().ReverseMap();
    }
    
}
