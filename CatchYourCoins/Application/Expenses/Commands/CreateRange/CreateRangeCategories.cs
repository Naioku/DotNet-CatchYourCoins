using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Commands;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentValidation;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.CreateRange;

public class CommandCreateRangeCategories : CommandCRUDCreateRange<InputDTOExpenseCategory>;

[UsedImplicitly]
public class ValidatorCreateRangeCategories : ValidatorCRUDCreateRange<CommandCreateRangeCategories, InputDTOExpenseCategory>
{
    public ValidatorCreateRangeCategories()
    {
        RuleFor(c => c.Data)
            .NotEmpty();
        
        RuleForEach(x => x.Data)
            .SetValidator(new ValidatorInputDTOExpenseCategory());
    }
}

public class HandlerCreateRangeCategories(
    IRepositoryExpenseCategory repository,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork)
    : HandlerCRUDCreateRange<ExpenseCategory, CommandCreateRangeCategories, InputDTOExpenseCategory>(repository, unitOfWork)
{
    protected override ExpenseCategory MapDTOToEntity(InputDTOExpenseCategory dto) =>
        new()
        {
            Name = dto.Name,
            Limit = dto.Limit,
            UserId = serviceCurrentUser.User.Id
        };
}