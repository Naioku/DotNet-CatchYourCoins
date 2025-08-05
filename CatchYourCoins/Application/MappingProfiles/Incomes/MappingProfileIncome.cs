using Application.Dashboard.DTOs.InputDTOs;
using Application.Dashboard.DTOs.InputDTOs.Incomes;
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
        CreateMap<InputDTOIncome, Income>()
            .IncludeBase<InputDTOFinancialOperation, FinancialOperation<IncomeCategory>>();
        
        CreateMap<Income, OutputDTOIncome>()
            .IncludeBase<FinancialOperation<IncomeCategory>, OutputDTOFinancialOperation>();
    }
}