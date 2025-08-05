using Application.Dashboard.DTOs.InputDTOs;
using Application.Dashboard.DTOs.OutputDTOs;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;

namespace Application.MappingProfiles;

public class MappingProfileFinancialCategory : Profile
{
    public MappingProfileFinancialCategory(IServiceCurrentUser serviceCurrentUser)
    {
        CreateMap<InputDTOFinancialCategory, FinancialCategory>()
            .ForMember(
                m => m.UserId,
                opt => opt.MapFrom(_ => serviceCurrentUser.User.Id)
            );
        
        CreateMap<FinancialCategory, OutputDTOFinancialCategory>();
    }
}