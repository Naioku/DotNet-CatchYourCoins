using Application.DTOs.Expenses;
using Application.Requests.Queries;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetAll;

public class QueryGetAllPaymentMethods : QueryGetAllBase<PaymentMethodDTO>;

public class HandlerGetAllPaymentMethods(IRepositoryExpensePaymentMethod repository)
    : HandlerCRUDGetAll<ExpensePaymentMethod, QueryGetAllPaymentMethods, PaymentMethodDTO>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "PaymentMethods", "Payment methods not found" }
        };

    protected override PaymentMethodDTO MapEntityToDTO(ExpensePaymentMethod entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}