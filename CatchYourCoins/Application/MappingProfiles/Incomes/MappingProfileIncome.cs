using Application.Dashboard.DTOs.CreateDTOs;
using Application.Dashboard.DTOs.CreateDTOs.Incomes;
using Application.Dashboard.DTOs.OutputDTOs;
using Application.Dashboard.DTOs.OutputDTOs.Incomes;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Incomes;

namespace Application.MappingProfiles.Incomes;

public class MappingProfileIncome : Profile
{
    public MappingProfileIncome()
    {
        CreateMap<CreateDTOIncome, Income>()
            .IncludeBase<CreateDTOFinancialOperation, FinancialOperation<IncomeCategory>>();
        
        CreateMap<Income, OutputDTOIncome>()
            .IncludeBase<FinancialOperation<IncomeCategory>, OutputDTOFinancialOperation>();
    }
}