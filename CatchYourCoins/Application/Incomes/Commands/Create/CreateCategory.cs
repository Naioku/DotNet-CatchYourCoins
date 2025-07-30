using Application.DTOs.InputDTOs.Incomes;
using Application.Requests.Commands;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;

namespace Application.Incomes.Commands.Create;

public class CommandCreateCategory : CommandCRUDCreate<InputDTOIncomeCategory>;

[UsedImplicitly]
public class ValidatorCreateCategory
    : ValidatorCRUDCreate<CommandCreateCategory, InputDTOIncomeCategory, ValidatorInputDTOIncomeCategory>;

public class HandlerCreateCategory(
    IRepositoryIncomeCategory repository,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork)
    : HandlerCRUDCreate<IncomeCategory, CommandCreateCategory, InputDTOIncomeCategory>(repository, unitOfWork)
{
    protected override IncomeCategory MapDTOToEntity(InputDTOIncomeCategory dto) =>
        new()
        {
            Name = dto.Name,
            Limit = dto.Limit,
            UserId = serviceCurrentUser.User.Id
        };
}