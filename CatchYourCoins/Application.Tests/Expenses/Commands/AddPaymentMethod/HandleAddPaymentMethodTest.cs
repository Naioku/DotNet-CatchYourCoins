using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Expenses.Commands;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
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
        const string name = "Test";
        const decimal limit = 1000;
        var command = new CommandAddPaymentMethod
        {
            Name = name,
            Limit = limit
        };

        var repositoryPaymentMethod = new Mock<IRepositoryPaymentMethod>();
        var serviceCurrentUser = new Mock<IServiceCurrentUser>();
        serviceCurrentUser.Setup(m => m.User)
            .Returns(new CurrentUser
            {
                Id = Guid.Parse("12345678-1234-1234-1234-123456789012"),
                Email = "test@example.com",
                Name = "Test User",
                IsAuthenticated = true
            });
        
        var unitOfWork = new Mock<IUnitOfWork>();
        HandlerAddPaymentMethod handlerAddPaymentMethod = new(
            repositoryPaymentMethod.Object,
            serviceCurrentUser.Object,
            unitOfWork.Object
        );
        
        // Act
        await handlerAddPaymentMethod.Handle(command, CancellationToken.None);

        // Assert
        repositoryPaymentMethod.Verify(m => m.CreatePaymentMethodAsync(It.IsAny<PaymentMethod>()), Times.Once);
        unitOfWork.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}