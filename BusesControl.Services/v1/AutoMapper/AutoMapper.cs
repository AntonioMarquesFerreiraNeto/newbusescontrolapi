using AutoMapper;
using BusesControl.Entities.Models;
using BusesControl.Entities.Request;
<<<<<<< HEAD
using BusesControl.Entities.Requests;
=======
>>>>>>> 3a9cc1cf2b23eafb9b2664df99d086fd4d58575f
using BusesControl.Entities.Responses;

namespace BusesControl.Services.v1.AutoMapper;

public class AutoMapper : Profile
{
    public AutoMapper() 
    {
        CreateMap<EmployeeModel, UserCreateRequest>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<SettingsPanelModel, SettingsPanelResponse>();
        CreateMap<UserModel, UserResponse>();
<<<<<<< HEAD

        CreateMap<CustomerCreateRequest, CustomerUpdateRequest>();
        CreateMap<CustomerUpdateRequest, CustomerCreateRequest>();
=======
>>>>>>> 3a9cc1cf2b23eafb9b2664df99d086fd4d58575f
    }
}
