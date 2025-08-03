using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Base.Commands;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Requests.Expenses.Commands.CreateRange;

public class CommandCreateRangePaymentMethods : CommandCRUDCreateRange<InputDTOExpensePaymentMethod>;

[UsedImplicitly]
public class ValidatorCreateRangePaymentMethods
    : ValidatorCRUDCreateRange<
        CommandCreateRangePaymentMethods,
        InputDTOExpensePaymentMethod,
        ValidatorInputDTOExpensePaymentMethod
    >;

public class HandlerCreateRangePaymentMethods(
    IRepositoryExpensePaymentMethod repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : HandlerCRUDCreateRange<
        ExpensePaymentMethod,
        CommandCreateRangePaymentMethods,
        InputDTOExpensePaymentMethod
    >(repository, unitOfWork, mapper);