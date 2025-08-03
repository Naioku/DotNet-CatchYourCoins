using Application.DTOs.InputDTOs.Incomes;
using Application.Requests.Commands;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Incomes.Commands.Create;

public class CommandCreateIncome : CommandCRUDCreate<InputDTOIncome>;

[UsedImplicitly]
public class ValidatorCreateIncome
    : ValidatorCRUDCreate<CommandCreateIncome, InputDTOIncome, ValidatorInputDTOIncome>;

public class HandlerCreateIncome(
    IRepositoryIncome repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : HandlerCRUDCreate<
        Income,
        CommandCreateIncome,
        InputDTOIncome
    >(repository, unitOfWork, mapper);