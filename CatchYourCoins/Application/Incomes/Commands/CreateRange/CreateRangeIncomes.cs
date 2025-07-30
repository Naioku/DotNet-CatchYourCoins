using Application.DTOs.InputDTOs.Incomes;
using Application.Requests.Commands;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;

namespace Application.Incomes.Commands.CreateRange;

public class CommandCreateRangeIncomes : CommandCRUDCreateRange<InputDTOIncome>;

[UsedImplicitly]
public class ValidatorCreateRangeIncomes
    : ValidatorCRUDCreateRange<
        CommandCreateRangeIncomes,
        InputDTOIncome,
        ValidatorInputDTOIncome
    >;

public class HandlerCreateRangeIncomes(
    IRepositoryIncome repository,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork)
    : HandlerCRUDCreateRange<Income, CommandCreateRangeIncomes, InputDTOIncome>(repository, unitOfWork)
{
    protected override Income MapDTOToEntity(InputDTOIncome dto) =>
        new()
        {
            Amount = dto.Amount,
            Date = dto.Date,
            Description = dto.Description,
            UserId = serviceCurrentUser.User.Id,
            CategoryId = dto.CategoryId,
        };
}