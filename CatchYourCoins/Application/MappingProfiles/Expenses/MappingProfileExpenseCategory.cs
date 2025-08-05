using Application.Dashboard.DTOs.InputDTOs;
using Application.Dashboard.DTOs.InputDTOs.Expenses;
using Application.Dashboard.DTOs.OutputDTOs;
using Application.Dashboard.DTOs.OutputDTOs.Expenses;
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