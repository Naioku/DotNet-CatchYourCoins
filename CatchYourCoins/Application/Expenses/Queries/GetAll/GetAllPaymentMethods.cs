using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Queries;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetAll;

public class QueryGetAllPaymentMethods : QueryCRUDGetAll<OutputDTOExpensePaymentMethod>;

public class HandlerGetAllPaymentMethods(
    IRepositoryExpensePaymentMethod repository,
    IMapper mapper)
    : HandlerCRUDGetAll<
        ExpensePaymentMethod,
        QueryGetAllPaymentMethods,
        OutputDTOExpensePaymentMethod
    >(repository, mapper)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "PaymentMethods", "Payment methods not found" }
        };
}