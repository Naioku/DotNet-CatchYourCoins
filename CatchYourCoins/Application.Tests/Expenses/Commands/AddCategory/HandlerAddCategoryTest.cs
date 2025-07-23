using System.Threading;
using System.Threading.Tasks;
using Application.Expenses.Commands;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Commands.AddCategory;

[TestSubject(typeof(HandlerAddCategory))]
public class HandlerAddCategoryTest
{
    [Fact]
    public async Task AddCategory_ValidData_CreateCategory()
    {
        // Arrange
        var command = new CommandAddCategory
        {
            Name = "Test",
            Limit = 1000
        };

        var repositoryCategory = new Mock<IRepositoryCategory>();
        var serviceCurrentUser = TestFactoryUsers.MockServiceCurrentUser();

        var unitOfWork = new Mock<IUnitOfWork>();
        HandlerAddCategory handlerAddCategory = new(
            repositoryCategory.Object,
            serviceCurrentUser.Object,
            unitOfWork.Object
        );

        // Act
        await handlerAddCategory.Handle(command, CancellationToken.None);

        // Assert
        repositoryCategory.Verify(m => m.CreateCategoryAsync(
                It.Is<Category>(c =>
                    c.Name == command.Name &&
                    c.Limit == command.Limit &&
                    c.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id)),
            Times.Once
        );
        unitOfWork.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}