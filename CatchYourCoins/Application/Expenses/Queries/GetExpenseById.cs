using Application.DTOs.Expenses;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Expenses.Queries;

public class QueryGetExpenseById : IRequest<Result<ExpenseDTO>>
{
    public required int Id { get; init; }
}

public class HandlerGetExpenseById(IRepositoryExpense repositoryExpense) : IRequestHandler<QueryGetExpenseById, Result<ExpenseDTO>>
{
    public async Task<Result<ExpenseDTO>> Handle(QueryGetExpenseById request, CancellationToken cancellationToken)
    {
        Expense? expense = await repositoryExpense.GetByIdAsync(request.Id);

        if (expense == null)
        {
            return Result<ExpenseDTO>.Failure(new Dictionary<string, string> { { "Expense", "Expense not found" } });
        }

        return Result<ExpenseDTO>.SetValue(new ExpenseDTO
        {
            Id = expense.Id,
            Amount = expense.Amount,
            Date = expense.Date,
            Description = expense.Description,
            Category = expense.Category?.Name,
            PaymentMethod = expense.PaymentMethod?.Name,
        });
    }
}