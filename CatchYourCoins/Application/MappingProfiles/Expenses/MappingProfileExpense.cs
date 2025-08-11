using Application.Dashboard.DTOs.InputDTOs;
using Application.Dashboard.DTOs.InputDTOs.Expenses;
using Application.Dashboard.DTOs.OutputDTOs;
using Application.Dashboard.DTOs.OutputDTOs.Expenses;
using Application.Dashboard.DTOs.UpdateDTOs;
using Application.Dashboard.DTOs.UpdateDTOs.Expenses;
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
            .IncludeBase<FinancialOperation<ExpenseCategory>, OutputDTOFinancialOperation>()
            .ForMember(
                m => m.PaymentMethod,
                opt => opt.MapFrom(src => src.PaymentMethod != null
                    ? src.PaymentMethod.Name
                    : null
                )
            );

        CreateMap<UpdateDTOExpense, Expense>()
            .IncludeBase<UpdateDTOFinancialOperation, FinancialOperation<ExpenseCategory>>()
            .ForMember(
                m => m.PaymentMethodId,
                opt => opt.MapFrom((src, dest, _, context) =>
                    src.PaymentMethodId.HasValue
                        ? src.PaymentMethodId.Value
                        : dest.PaymentMethodId
                )
            )
            .ForMember(
                m => m.PaymentMethod,
                opt => opt.MapFrom((src, dest, _, context) =>
                    src.PaymentMethodId.HasValue
                        ? null
                        : dest.PaymentMethod
                )
            );
    }
}