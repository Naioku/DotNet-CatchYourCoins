using Application.DTOs.InputDTOs.Incomes;
using Application.Requests.Commands;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;

namespace Application.Incomes.Commands.Create;

public class CommandCreateIncome : CommandCRUDCreate<InputDTOIncome>;

[UsedImplicitly]
public class ValidatorCreateIncome
    : ValidatorCRUDCreate<CommandCreateIncome, InputDTOIncome, ValidatorInputDTOIncome>;

public class HandlerCreateIncome(
    IRepositoryIncome repository,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork)
    : HandlerCRUDCreate<Income, CommandCreateIncome, InputDTOIncome>(repository, unitOfWork)
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