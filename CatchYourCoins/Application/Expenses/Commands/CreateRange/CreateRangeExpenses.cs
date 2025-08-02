using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Commands;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.CreateRange;

public class CommandCreateRangeExpenses : CommandCRUDCreateRange<InputDTOExpense>;

[UsedImplicitly]
public class ValidatorCreateRangeExpenses
    : ValidatorCRUDCreateRange<
        CommandCreateRangeExpenses,
        InputDTOExpense,
        ValidatorInputDTOExpense
    >;

public class HandlerCreateRangeExpenses(
    IRepositoryExpense repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : HandlerCRUDCreateRange<
        Expense,
        CommandCreateRangeExpenses,
        InputDTOExpense
    >(repository, unitOfWork, mapper);