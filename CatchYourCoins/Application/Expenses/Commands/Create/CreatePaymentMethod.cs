using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Commands;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Create;

public class CommandCreatePaymentMethod : CommandCRUDCreate<InputDTOExpensePaymentMethod>;

[UsedImplicitly]
public class ValidatorCreatePaymentMethod
    : ValidatorCRUDCreate<
        CommandCreatePaymentMethod,
        InputDTOExpensePaymentMethod,
        ValidatorInputDTOExpensePaymentMethod
    >;

public class HandlerCreatePaymentMethod(
    IRepositoryExpensePaymentMethod repository,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : HandlerCRUDCreate<
        ExpensePaymentMethod,
        CommandCreatePaymentMethod,
        InputDTOExpensePaymentMethod
    >(repository, unitOfWork, mapper);