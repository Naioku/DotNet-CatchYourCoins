using Application.DTOs.InputDTOs;
using Application.DTOs.InputDTOs.Incomes;
using Application.DTOs.OutputDTOs;
using Application.DTOs.OutputDTOs.Incomes;
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