using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Base.Commands;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Requests.Expenses.Commands.Create;

public class CommandCreateExpense : CommandCRUDCreate<InputDTOExpense>;

[UsedImplicitly]
public class ValidatorCreateExpense
    : ValidatorCRUDCreate<
        CommandCreateExpense,
        InputDTOExpense,
        ValidatorInputDTOExpense
    >;

public class HandlerCreateExpense(
    IRepositoryExpense repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : HandlerCRUDCreate<
        Expense,
        CommandCreateExpense,
        InputDTOExpense
    >(repository, unitOfWork, mapper);