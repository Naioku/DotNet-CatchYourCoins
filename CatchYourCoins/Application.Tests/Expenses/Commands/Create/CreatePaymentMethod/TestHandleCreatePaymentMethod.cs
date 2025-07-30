using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Expenses;
using Application.Expenses.Commands.Create;
using Application.Tests.Factories;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Create.CreatePaymentMethod;

[TestSubject(typeof(HandlerCreatePaymentMethod))]
public class TestHandlerCreatePaymentMethod
    : TestHandlerCreate<
        HandlerCreatePaymentMethod,
        ExpensePaymentMethod,
        InputDTOExpensePaymentMethod,
        CommandCreatePaymentMethod,
        IRepositoryExpensePaymentMethod,
        TestFactoryPaymentMethod
    >
{
    protected override HandlerCreatePaymentMethod CreateHandler()
    {
        return new HandlerCreatePaymentMethod(
            GetMock<IRepositoryExpensePaymentMethod>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }
    
    protected override CommandCreatePaymentMethod GetCommand() =>
        new()
        {
            Data = new InputDTOExpensePaymentMethod
            {
                Name = "Test",
                Limit = 1000
            }
        };

    protected override Expression<Func<ExpensePaymentMethod, bool>> GetRepositoryMatch(CommandCreatePaymentMethod command) =>
        pm =>
            pm.Name == command.Data.Name &&
            pm.Limit == command.Data.Limit &&
            pm.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id;

    [Fact]
    public async Task Create_ValidData_EntryCreated() =>
        await Create_ValidData_EntityCreated_Base();
}