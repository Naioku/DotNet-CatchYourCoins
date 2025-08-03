using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Base.Commands;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Requests.Expenses.Commands.CreateRange;

public class CommandCreateRangeCategories : CommandCRUDCreateRange<InputDTOExpenseCategory>;

[UsedImplicitly]
public class ValidatorCreateRangeCategories
    : ValidatorCRUDCreateRange<
        CommandCreateRangeCategories,
        InputDTOExpenseCategory,
        ValidatorInputDTOExpenseCategory
    >;

public class HandlerCreateRangeCategories(
    IRepositoryExpenseCategory repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : HandlerCRUDCreateRange<
        ExpenseCategory,
        CommandCreateRangeCategories,
        InputDTOExpenseCategory
    >(repository, unitOfWork, mapper);