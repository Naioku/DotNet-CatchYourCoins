using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Expenses.Commands.Create;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Create.CreatePaymentMethod;

[TestSubject(typeof(Application.Expenses.Commands.Create.TestHandlerCreatePaymentMethod))]
public class TestHandlerCreatePaymentMethod
    : TestHandlerCreate<
        Application.Expenses.Commands.Create.TestHandlerCreatePaymentMethod,
        PaymentMethod,
        CommandCreatePaymentMethod,
        IRepositoryPaymentMethod,
        TestFactoryPaymentMethod
    >
{
    protected override Application.Expenses.Commands.Create.TestHandlerCreatePaymentMethod CreateHandler()
    {
        return new Application.Expenses.Commands.Create.TestHandlerCreatePaymentMethod(
            GetMock<IRepositoryPaymentMethod>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }
    
    protected override CommandCreatePaymentMethod GetCommand() =>
        new()
        {
            Name = "Test",
            Limit = 1000
        };

    protected override Expression<Func<PaymentMethod, bool>> GetRepositoryMatch(CommandCreatePaymentMethod command) =>
        pm =>
            pm.Name == command.Name &&
            pm.Limit == command.Limit &&
            pm.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id;

    [Fact]
    public async Task Create_ValidData_EntryCreated() =>
        await Create_ValidData_EntityCreated_Base();
}