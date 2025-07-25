using System.Threading;
using System.Threading.Tasks;
using Application.Expenses.Commands;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Commands.CreateCategory;

[TestSubject(typeof(HandlerCreateCategory))]
public class HandlerCreateCategoryTest : CQRSHandlerTestBase<HandlerCreateCategory>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryCategory>();
        RegisterMock<IUnitOfWork>();
        return base.InitializeAsync();
    }

    protected override HandlerCreateCategory CreateHandler()
    {
        return new HandlerCreateCategory(
            GetMock<IRepositoryCategory>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }
    
    [Fact]
    public async Task Create_ValidData_EntityCreated()
    {
        // Arrange
        var command = new CommandCreateCategory
        {
            Name = "Test",
            Limit = 1000
        };

        // Act
        await Handler.Handle(command, CancellationToken.None);

        // Assert
        GetMock<IRepositoryCategory>().Verify(m => m.CreateAsync(
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