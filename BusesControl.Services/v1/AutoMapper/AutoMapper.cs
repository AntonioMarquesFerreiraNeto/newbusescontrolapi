using AutoMapper;
using BusesControl.Entities.Models;
using BusesControl.Entities.Request;

namespace BusesControl.Services.v1.AutoMapper;

public class AutoMapper : Profile
{
    public AutoMapper() 
    {
        CreateMap<EmployeeModel, UserCreateRequest>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Type.ToString()));
    }
}
