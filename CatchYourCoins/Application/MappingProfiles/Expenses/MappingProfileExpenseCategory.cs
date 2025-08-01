using Application.DTOs.InputDTOs;
using Application.DTOs.InputDTOs.Expenses;
using Application.DTOs.OutputDTOs;
using Application.DTOs.OutputDTOs.Expenses;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Expenses;

namespace Application.MappingProfiles.Expenses;

public class MappingProfileExpenseCategory : Profile
{
    public MappingProfileExpenseCategory()
    {
        CreateMap<InputDTOExpenseCategory, ExpenseCategory>()
            .IncludeBase<InputDTOFinancialCategory, FinancialCategory>();
        
        CreateMap<ExpenseCategory, OutputDTOExpenseCategory>()
            .IncludeBase<FinancialCategory, OutputDTOFinancialCategory>();
    }
}