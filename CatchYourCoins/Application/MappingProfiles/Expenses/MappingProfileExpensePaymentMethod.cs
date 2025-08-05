using Application.Dashboard.DTOs.InputDTOs;
using Application.Dashboard.DTOs.InputDTOs.Expenses;
using Application.Dashboard.DTOs.OutputDTOs;
using Application.Dashboard.DTOs.OutputDTOs.Expenses;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Expenses;

namespace Application.MappingProfiles.Expenses;

public class MappingProfileExpensePaymentMethod : Profile
{
    public MappingProfileExpensePaymentMethod()
    {
        CreateMap<InputDTOExpensePaymentMethod, ExpensePaymentMethod>()
            .IncludeBase<InputDTOFinancialCategory, FinancialCategory>();
        
        CreateMap<ExpensePaymentMethod, OutputDTOExpensePaymentMethod>()
            .IncludeBase<FinancialCategory, OutputDTOFinancialCategory>();
    }
}