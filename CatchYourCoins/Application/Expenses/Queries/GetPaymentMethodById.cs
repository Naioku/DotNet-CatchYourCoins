using Application.DTOs.Expenses;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Expenses.Queries;

public class QueryGetPaymentMethodById : IRequest<Result<PaymentMethodDTO>>
{
    public required int Id { get; init; }
}

public class HandlerGetPaymentMethodById(IRepositoryPaymentMethod repositoryPaymentMethod) : IRequestHandler<QueryGetPaymentMethodById, Result<PaymentMethodDTO>>
{
    public async Task<Result<PaymentMethodDTO>> Handle(QueryGetPaymentMethodById request, CancellationToken cancellationToken)
    {
        PaymentMethod? category = await repositoryPaymentMethod.GetPaymentMethodByIdAsync(request.Id);

        if (category == null)
        {
            return Result<PaymentMethodDTO>.Failure(new Dictionary<string, string> { { "Category", "Category not found" } });
        }
        
        return Result<PaymentMethodDTO>.SetValue(new PaymentMethodDTO
        {
            Id = category.Id,
            Name = category.Name,
            Limit = category.Limit,
        });
    }
}