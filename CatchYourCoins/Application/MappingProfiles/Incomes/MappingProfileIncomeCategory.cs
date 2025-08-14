using Application.Dashboard.DTOs.CreateDTOs;
using Application.Dashboard.DTOs.CreateDTOs.Incomes;
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
        CreateMap<CreateDTOIncomeCategory, IncomeCategory>()
            .IncludeBase<CreateDTOFinancialCategory, FinancialCategory>();
        
        CreateMap<IncomeCategory, OutputDTOIncomeCategory>()
            .IncludeBase<FinancialCategory, OutputDTOFinancialCategory>();
    }
}