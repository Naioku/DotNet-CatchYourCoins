using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Queries;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetById;

public class QueryGetPaymentMethodById : QueryCRUDGetById<OutputDTOExpensePaymentMethod>;

public class HandlerGetPaymentMethodById(IRepositoryExpensePaymentMethod repository)
    : HandlerCRUDGetById<ExpensePaymentMethod, QueryGetPaymentMethodById, OutputDTOExpensePaymentMethod>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "PaymentMethod", "Payment method not found" }
        };

    protected override OutputDTOExpensePaymentMethod MapEntityToDTO(ExpensePaymentMethod entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}