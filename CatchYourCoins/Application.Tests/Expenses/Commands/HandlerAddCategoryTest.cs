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

namespace Application.Tests.Expenses.Commands;

[TestSubject(typeof(HandlerAddCategory))]
public class HandlerAddCategoryTest
{
    [Fact]
    public async Task AddCategory_WithValidData_ShouldCreateCategory()
    {
        // Arrange
        const string name = "Test";
        const decimal limit = 1000;
        var command = new CommandAddCategory
        {
            Name = name,
            Limit = limit
        };

        var repositoryCategory = new Mock<IRepositoryCategory>();
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
        HandlerAddCategory handlerAddCategory = new(
            repositoryCategory.Object,
            serviceCurrentUser.Object,
            unitOfWork.Object
        );

        // Act
        await handlerAddCategory.Handle(command, CancellationToken.None);

        // Assert
        repositoryCategory.Verify(m => m.CreateCategoryAsync(It.IsAny<Category>()), Times.Once);
        unitOfWork.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}