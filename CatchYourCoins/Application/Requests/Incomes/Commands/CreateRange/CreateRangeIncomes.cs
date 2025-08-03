using Application.DTOs.InputDTOs.Incomes;
using Application.Requests.Base.Commands;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Requests.Incomes.Commands.CreateRange;

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
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : HandlerCRUDCreateRange<
        Income,
        CommandCreateRangeIncomes,
        InputDTOIncome
    >(repository, unitOfWork, mapper);