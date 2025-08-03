using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Commands;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Create;

public class CommandCreateCategory : CommandCRUDCreate<InputDTOExpenseCategory>;

[UsedImplicitly]
public class ValidatorCreateCategory
    : ValidatorCRUDCreate<
        CommandCreateCategory,
        InputDTOExpenseCategory,
        ValidatorInputDTOExpenseCategory
    >;

public class HandlerCreateCategory(
    IRepositoryExpenseCategory repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : HandlerCRUDCreate<
        ExpenseCategory,
        CommandCreateCategory,
        InputDTOExpenseCategory
    >(repository, unitOfWork, mapper);