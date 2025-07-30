using Application.DTOs.InputDTOs.Incomes;
using Application.Requests.Commands;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;

namespace Application.Incomes.Commands.CreateRange;

public class CommandCreateRangeCategories : CommandCRUDCreateRange<InputDTOIncomeCategory>;

[UsedImplicitly]
public class ValidatorCreateRangeCategories
    : ValidatorCRUDCreateRange<
        CommandCreateRangeCategories,
        InputDTOIncomeCategory,
        ValidatorInputDTOIncomeCategory
    >;

public class HandlerCreateRangeCategories(
    IRepositoryIncomeCategory repository,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork)
    : HandlerCRUDCreateRange<IncomeCategory, CommandCreateRangeCategories, InputDTOIncomeCategory>(repository, unitOfWork)
{
    protected override IncomeCategory MapDTOToEntity(InputDTOIncomeCategory dto) =>
        new()
        {
            Name = dto.Name,
            Limit = dto.Limit,
            UserId = serviceCurrentUser.User.Id
        };
}