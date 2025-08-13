using Application.Dashboard.DTOs.CreateDTOs;
using Application.Dashboard.DTOs.CreateDTOs.Expenses;
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
        CreateMap<CreateDTOExpensePaymentMethod, ExpensePaymentMethod>()
            .IncludeBase<CreateDTOFinancialCategory, FinancialCategory>();
        
        CreateMap<ExpensePaymentMethod, OutputDTOExpensePaymentMethod>()
            .IncludeBase<FinancialCategory, OutputDTOFinancialCategory>();
    }
}