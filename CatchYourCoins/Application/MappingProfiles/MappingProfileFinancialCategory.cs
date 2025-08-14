using Application.Dashboard.DTOs;
using Application.Dashboard.DTOs.CreateDTOs;
using Application.Dashboard.DTOs.OutputDTOs;
using Application.Dashboard.DTOs.UpdateDTOs;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;

namespace Application.MappingProfiles;

public class MappingProfileFinancialCategory : Profile
{
    public MappingProfileFinancialCategory(IServiceCurrentUser serviceCurrentUser)
    {
        CreateMap<CreateDTOFinancialCategory, FinancialCategory>()
            .IncludeBase<IInputDTODashboardEntity, DashboardEntity>();

        CreateMap<FinancialCategory, OutputDTOFinancialCategory>();

        CreateMap<UpdateDTOFinancialCategory, FinancialCategory>()
            .IncludeBase<IInputDTODashboardEntity, DashboardEntity>()
            .ForMember(
                m => m.Name,
                opt => opt.MapFrom((src, dest) => src.Name.HasValue ? src.Name.Value : dest.Name)
            )
            .ForMember(
                m => m.Limit,
                opt => opt.MapFrom((src, dest) => src.Limit.HasValue ? src.Limit.Value : dest.Limit)
            );
    }
}