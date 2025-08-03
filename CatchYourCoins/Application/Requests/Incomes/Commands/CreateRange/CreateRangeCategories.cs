using Application.DTOs.InputDTOs.Incomes;
using Application.Requests.Base.Commands;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Requests.Incomes.Commands.CreateRange;

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
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : HandlerCRUDCreateRange<
        IncomeCategory,
        CommandCreateRangeCategories,
        InputDTOIncomeCategory
    >(repository, unitOfWork, mapper);