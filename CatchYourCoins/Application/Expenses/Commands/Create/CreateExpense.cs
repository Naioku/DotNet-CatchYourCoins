using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Commands;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Create;

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
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : HandlerCRUDCreate<
        Expense,
        CommandCreateExpense,
        InputDTOExpense
    >(repository, unitOfWork, mapper);