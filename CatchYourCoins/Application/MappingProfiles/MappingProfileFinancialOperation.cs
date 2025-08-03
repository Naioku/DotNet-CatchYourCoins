using Application.DTOs.InputDTOs;
using Application.DTOs.OutputDTOs;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;

namespace Application.MappingProfiles;

public class MappingProfileFinancialOperation : Profile
{
    public MappingProfileFinancialOperation(IServiceCurrentUser serviceCurrentUser)
    {
        CreateMap(typeof(InputDTOFinancialOperation), typeof(FinancialOperation<>))
            .ForMember(
                "UserId",
                opt => opt.MapFrom(_ => serviceCurrentUser.User.Id)
            );

        CreateMap(typeof(FinancialOperation<>), typeof(OutputDTOFinancialOperation))
            .ForMember(
                "Category",
                opt => opt.MapFrom("Category.Name")
            );
    }
}