using Application.DTOs.Expenses;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Expenses.Queries;

public class QueryGetCategoryById : IRequest<Result<CategoryDTO>>
{
    public required int Id { get; init; }
}

public class HandlerGetCategoryById(IRepositoryCategory repositoryCategory) : IRequestHandler<QueryGetCategoryById, Result<CategoryDTO>>
{
    public async Task<Result<CategoryDTO>> Handle(QueryGetCategoryById request, CancellationToken cancellationToken)
    {
        Category? category = await repositoryCategory.GetCategoryByIdAsync(request.Id);

        if (category == null)
        {
            return Result<CategoryDTO>.Failure(new Dictionary<string, string> { { "Category", "Category not found" } });
        }
        
        return Result<CategoryDTO>.SetValue(new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Limit = category.Limit,
        });
    }
}