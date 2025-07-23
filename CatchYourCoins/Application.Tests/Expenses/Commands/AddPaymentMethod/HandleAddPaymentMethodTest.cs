using System.Threading;
using System.Threading.Tasks;
using Application.Expenses.Commands;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Commands.AddPaymentMethod;

[TestSubject(typeof(HandlerAddPaymentMethod))]
public class HandlerAddPaymentMethodTest
{
    [Fact]
    public async Task AddPaymentMethod_ValidData_CreatePaymentMethod()
    {
        // Arrange
        var command = new CommandAddPaymentMethod
        {
            Name = "Test",
            Limit = 1000
        };

        var repositoryPaymentMethod = new Mock<IRepositoryPaymentMethod>();
        var serviceCurrentUser = TestFactoryUsers.MockServiceCurrentUser();

        var unitOfWork = new Mock<IUnitOfWork>();
        HandlerAddPaymentMethod handlerAddPaymentMethod = new(
            repositoryPaymentMethod.Object,
            serviceCurrentUser.Object,
            unitOfWork.Object
        );

        // Act
        await handlerAddPaymentMethod.Handle(command, CancellationToken.None);

        // Assert
        repositoryPaymentMethod.Verify(m => m.CreatePaymentMethodAsync(
                It.Is<PaymentMethod>(pm => 
                    pm.Name == command.Name &&
                    pm.Limit == command.Limit &&
                    pm.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id)),
            Times.Once
        );
        unitOfWork.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}