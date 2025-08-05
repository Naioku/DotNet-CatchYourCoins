using Application.Dashboard.DTOs.InputDTOs;
using Application.Dashboard.DTOs.InputDTOs.Incomes;
using Application.Dashboard.DTOs.OutputDTOs;
using Application.Dashboard.DTOs.OutputDTOs.Incomes;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Incomes;

namespace Application.MappingProfiles.Incomes;

public class MappingProfileIncomeCategory : Profile
{
    public MappingProfileIncomeCategory()
    {
        CreateMap<InputDTOIncomeCategory, IncomeCategory>()
            .IncludeBase<InputDTOFinancialCategory, FinancialCategory>();
        
        CreateMap<IncomeCategory, OutputDTOIncomeCategory>()
            .IncludeBase<FinancialCategory, OutputDTOFinancialCategory>();
    }
}