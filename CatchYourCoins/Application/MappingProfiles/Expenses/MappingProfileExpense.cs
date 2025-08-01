using Application.DTOs.InputDTOs;
using Application.DTOs.InputDTOs.Expenses;
using Application.DTOs.OutputDTOs;
using Application.DTOs.OutputDTOs.Expenses;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Expenses;

namespace Application.MappingProfiles.Expenses;

public class MappingProfileExpense : Profile
{
    public MappingProfileExpense()
    {
        CreateMap<InputDTOExpense, Expense>()
            .IncludeBase<InputDTOFinancialOperation, FinancialOperation<ExpenseCategory>>();
        
        CreateMap<Expense, OutputDTOExpense>()
            .IncludeBase<FinancialOperation<ExpenseCategory>, OutputDTOFinancialOperation>();
    }
}