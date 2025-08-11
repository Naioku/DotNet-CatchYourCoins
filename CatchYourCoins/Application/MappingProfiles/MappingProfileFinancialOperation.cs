using Application.Dashboard.DTOs.InputDTOs;
using Application.Dashboard.DTOs.OutputDTOs;
using Application.Dashboard.DTOs.UpdateDTOs;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;

namespace Application.MappingProfiles;

public class MappingProfileFinancialOperation : Profile
{
    public MappingProfileFinancialOperation(IServiceCurrentUser serviceCurrentUser)
    {
        // Todo: Create base mapper.
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

        CreateMap(typeof(UpdateDTOFinancialOperation), typeof(FinancialOperation<>))
            .ForMember(
                "UserId",
                opt => opt.MapFrom(_ => serviceCurrentUser.User.Id)
            )
            .ForMember(
                "Amount",
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    UpdateDTOFinancialOperation typedSrc = (UpdateDTOFinancialOperation)src;
                    dynamic typedDest = dest;
                    return typedSrc.Amount.HasValue
                        ? typedSrc.Amount.Value
                        : typedDest.Amount;
                })
            )
            .ForMember(
                "Date",
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    var typedSrc = (UpdateDTOFinancialOperation)src;
                    var typedDest = (dynamic)dest;
                    return typedSrc.Date.HasValue
                        ? typedSrc.Date.Value
                        : typedDest.Date;
                })
            )
            .ForMember(
                "Description",
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    var typedSrc = (UpdateDTOFinancialOperation)src;
                    var typedDest = (dynamic)dest;
                    return typedSrc.Description.HasValue
                        ? typedSrc.Description.Value
                        : typedDest.Description;
                })
            )
            .ForMember(
                "CategoryId",
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    var typedSrc = (UpdateDTOFinancialOperation)src;
                    var typedDest = (dynamic)dest;
                    return typedSrc.CategoryId.HasValue
                        ? typedSrc.CategoryId.Value
                        : typedDest.CategoryId;
                })
            )
            .ForMember(
                "Category",
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    var typedSrc = (UpdateDTOFinancialOperation)src;
                    var typedDest = (dynamic)dest;
                    return typedSrc.CategoryId.HasValue
                        ? null
                        : typedDest.Category;
                })
            );
    }
}