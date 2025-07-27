using System.Threading.Tasks;
using Application.Expenses.Commands.Delete;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Delete.DeletePaymentMethod;

[TestSubject(typeof(HandlerDeletePaymentMethod))]
public class TestHandlerDeletePaymentMethod
    : TestHandlerDelete<HandlerDeletePaymentMethod, PaymentMethod, CommandDeletePaymentMethod, IRepositoryPaymentMethod, TestFactoryPaymentMethod, IUnitOfWork>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryPaymentMethod>();
        RegisterMock<IUnitOfWork>();
        return base.InitializeAsync();
    }

    protected override HandlerDeletePaymentMethod CreateHandler()
    {
        return new HandlerDeletePaymentMethod(
            GetMock<IRepositoryPaymentMethod>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }
    
    protected override CommandDeletePaymentMethod GetCommand() => new() { Id = 1 };

    [Fact]
    public async Task DeleteOne_ValidData_DeletedEntity() =>
        await DeleteOne_ValidData_DeletedEntity_Base();

    [Fact]
    public async Task DeleteOne_NoEntryAtPassedID_DeletedNothing() =>
        await DeleteOne_NoEntryAtPassedID_DeletedNothing_Base();
}