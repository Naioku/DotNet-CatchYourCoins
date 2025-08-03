using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Expenses;
using Application.Expenses.Commands.CreateRange;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.CreateRange.CreateRangePaymentMethods;

[TestSubject(typeof(HandlerCreateRangePaymentMethods))]
public class TestHandlerCreateRangePaymentMethods
    : TestHandlerCreateRange<
        HandlerCreateRangePaymentMethods,
        ExpensePaymentMethod,
        InputDTOExpensePaymentMethod,
        CommandCreateRangePaymentMethods,
        IRepositoryExpensePaymentMethod
    >
{
    protected override HandlerCreateRangePaymentMethods CreateHandler()
    {
        return new HandlerCreateRangePaymentMethods(
            GetMock<IRepositoryExpensePaymentMethod>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override CommandCreateRangePaymentMethods GetCommand(List<InputDTOExpensePaymentMethod> dtos) => new() { Data = dtos };


    [Fact]
    public async Task Create_ValidData_EntitiesCreated() =>
        await Create_ValidData_EntitiesCreated_Base();
    
    [Fact]
    public async Task Create_RepositoryThrowsException_EntityNotCreated() =>
        await Create_RepositoryThrowsException_EntitiesNotCreated_Base();
    
    [Fact]
    public async Task Create_UnitOfWorkThrowsException_EntitiesNotCreated() =>
        await Create_UnitOfWorkThrowsException_EntitiesNotCreated_Base();
}