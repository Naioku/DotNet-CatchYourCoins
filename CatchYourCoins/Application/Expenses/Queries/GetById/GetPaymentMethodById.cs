using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Queries;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetById;

public class QueryGetPaymentMethodById : QueryCRUDGetById<OutputDTOExpensePaymentMethod>;

public class HandlerGetPaymentMethodById(
    IRepositoryExpensePaymentMethod repository,
    IMapper mapper)
    : HandlerCRUDGetById<
        ExpensePaymentMethod,
        QueryGetPaymentMethodById,
        OutputDTOExpensePaymentMethod
    >(repository, mapper)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "PaymentMethod", "Payment method not found" }
        };
}