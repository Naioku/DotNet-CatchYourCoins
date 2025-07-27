using Application.DTOs.Expenses;
using Application.Requests.Queries.GetAll;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetAll;

public class QueryGetAllPaymentMethods : QueryGetAllBase<PaymentMethodDTO>;

public class HandlerGetAllPaymentMethods(IRepositoryPaymentMethod repository) : HandlerCRUDGetAll<PaymentMethod, QueryGetAllPaymentMethods, PaymentMethodDTO>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "PaymentMethods", "Payment methods not found" }
        };

    protected override PaymentMethodDTO MapEntityToDTO(PaymentMethod entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}