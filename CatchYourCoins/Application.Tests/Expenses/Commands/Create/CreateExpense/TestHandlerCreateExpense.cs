using System;
using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Expenses;
using Application.Expenses.Commands.Create;
using Application.Tests.Factories;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Create.CreateExpense;

[TestSubject(typeof(HandlerCreateExpense))]
public class TestHandlerCreateExpense
    : TestHandlerCreate<
        HandlerCreateExpense,
        Expense,
        InputDTOExpense,
        CommandCreateExpense,
        IRepositoryExpense,
        TestFactoryExpense
    >
{
    private readonly InputDTOExpense _dto = new()
    {
        Amount = 100,
        Date = DateTime.Now,
        Description = "Test",
        CategoryId = 1,
        PaymentMethodId = 1
    };

    protected override HandlerCreateExpense CreateHandler()
    {
        return new HandlerCreateExpense(
            GetMock<IRepositoryExpense>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override InputDTOExpense GetInputDTO() => _dto;
    protected override CommandCreateExpense GetCommand() => new() { Data = _dto };

    protected override Expense GetMappedEntity() => new()
    {
        Amount = _dto.Amount,
        Date = _dto.Date,
        Description = _dto.Description,
        CategoryId = _dto.CategoryId,
        PaymentMethodId = _dto.PaymentMethodId,
        UserId = TestFactoryUsers.DefaultUser1Authenticated.Id,
    };

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}