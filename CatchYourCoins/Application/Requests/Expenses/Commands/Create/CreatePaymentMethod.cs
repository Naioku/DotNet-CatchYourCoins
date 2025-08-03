using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Base.Commands;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Requests.Expenses.Commands.Create;

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
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : HandlerCRUDCreate<
        ExpensePaymentMethod,
        CommandCreatePaymentMethod,
        InputDTOExpensePaymentMethod
    >(repository, unitOfWork, mapper);