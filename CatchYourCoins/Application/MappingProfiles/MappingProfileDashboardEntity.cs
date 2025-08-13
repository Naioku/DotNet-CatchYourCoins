using Application.Dashboard.DTOs;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;

namespace Application.MappingProfiles;

public class MappingProfileDashboardEntity : Profile
{
    public MappingProfileDashboardEntity(IServiceCurrentUser serviceCurrentUser)
    {
        CreateMap<IInputDTODashboardEntity, DashboardEntity>()
            .ForMember(
                m => m.UserId,
                opt => opt.MapFrom(_ => serviceCurrentUser.User.Id)
            );
    }
}