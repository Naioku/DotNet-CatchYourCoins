using Application.Dashboard.DTOs.InputDTOs;
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
        // Todo: Create base mapper.
        CreateMap<InputDTOFinancialCategory, FinancialCategory>()
            .ForMember(
                m => m.UserId,
                opt => opt.MapFrom(_ => serviceCurrentUser.User.Id)
            );

        CreateMap<FinancialCategory, OutputDTOFinancialCategory>();

        CreateMap<UpdateDTOFinancialCategory, FinancialCategory>()
            .ForMember(
                m => m.UserId,
                opt => opt.MapFrom(_ => serviceCurrentUser.User.Id)
            )
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