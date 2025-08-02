using Application.DTOs.InputDTOs.Incomes;
using Application.Requests.Commands;
using AutoMapper;
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
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : HandlerCRUDCreate<
        IncomeCategory,
        CommandCreateCategory,
        InputDTOIncomeCategory
    >(repository, unitOfWork, mapper);