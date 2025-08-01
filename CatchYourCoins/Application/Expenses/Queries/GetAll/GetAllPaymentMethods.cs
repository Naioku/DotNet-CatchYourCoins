using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Queries;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetAll;

public class QueryGetAllPaymentMethods : QueryCRUDGetAll<OutputDTOExpensePaymentMethod>;

public class HandlerGetAllPaymentMethods(IRepositoryExpensePaymentMethod repository)
    : HandlerCRUDGetAll<ExpensePaymentMethod, QueryGetAllPaymentMethods, OutputDTOExpensePaymentMethod>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "PaymentMethods", "Payment methods not found" }
        };

    protected override OutputDTOExpensePaymentMethod MapEntityToDTO(ExpensePaymentMethod entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}