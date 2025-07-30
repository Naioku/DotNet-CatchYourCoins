using Application.DTOs.Expenses;
using Application.Requests.Queries;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetById;

public class QueryGetPaymentMethodById : QueryCRUDGetById<PaymentMethodDTO>;

public class HandlerGetPaymentMethodById(IRepositoryExpensePaymentMethod repository)
    : HandlerCRUDGetById<ExpensePaymentMethod, QueryGetPaymentMethodById, PaymentMethodDTO>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "PaymentMethod", "Payment method not found" }
        };

    protected override PaymentMethodDTO MapEntityToDTO(ExpensePaymentMethod entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}