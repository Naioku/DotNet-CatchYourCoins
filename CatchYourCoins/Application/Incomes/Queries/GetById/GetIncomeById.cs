using Application.DTOs.OutputDTOs.Incomes;
using Application.Requests.Queries;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;

namespace Application.Incomes.Queries.GetById;

public class QueryGetIncomeById : QueryCRUDGetById<OutputDTOIncome>;

public class HandlerGetIncomeById(IRepositoryIncome repository)
    : HandlerCRUDGetById<Domain.Dashboard.Entities.Incomes.Income, QueryGetIncomeById, OutputDTOIncome>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Income", "Income not found" }
        };

    protected override OutputDTOIncome MapEntityToDTO(Domain.Dashboard.Entities.Incomes.Income entity) =>
        new()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            Date = entity.Date,
            Description = entity.Description,
            Category = entity.Category?.Name,
        };
}