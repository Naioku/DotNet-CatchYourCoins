using System.Threading;
using System.Threading.Tasks;
using Application.Expenses.Commands;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Commands.AddCategory;

[TestSubject(typeof(HandlerAddCategory))]
public class HandlerAddCategoryTest : CQRSHandlerTestBase<HandlerAddCategory>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryCategory>();
        RegisterMock<IUnitOfWork>();
        return base.InitializeAsync();
    }

    protected override HandlerAddCategory CreateHandler()
    {
        return new HandlerAddCategory(
            GetMock<IRepositoryCategory>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }
    
    [Fact]
    public async Task AddCategory_ValidData_CreateCategory()
    {
        // Arrange
        var command = new CommandAddCategory
        {
            Name = "Test",
            Limit = 1000
        };

        // Act
        await Handler.Handle(command, CancellationToken.None);

        // Assert
        GetMock<IRepositoryCategory>().Verify(m => m.CreateCategoryAsync(
                It.Is<Category>(c =>
                    c.Name == command.Name &&
                    c.Limit == command.Limit &&
                    c.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id)),
            Times.Once
        );
        GetMock<IUnitOfWork>().Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}