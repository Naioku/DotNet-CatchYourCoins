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

namespace Application.Tests.Expenses.Commands.Create.CreateCategory;

[TestSubject(typeof(HandlerCreateCategory))]
public class TestHandlerCreateCategory
    : TestHandlerCreate<
        HandlerCreateCategory,
        ExpenseCategory,
        InputDTOExpenseCategory,
        CommandCreateCategory,
        IRepositoryExpenseCategory,
        TestFactoryExpenseCategory
    >
{
    private readonly InputDTOExpenseCategory _dto = new()
    {
        Name = "Test1",
        Limit = 1000
    };

    protected override HandlerCreateCategory CreateHandler()
    {
        return new HandlerCreateCategory(
            GetMock<IRepositoryExpenseCategory>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override InputDTOExpenseCategory GetInputDTO() => _dto;
    protected override CommandCreateCategory GetCommand() => new() { Data = _dto };

    protected override ExpenseCategory GetMappedEntity() => new()
    {
        Name = _dto.Name,
        Limit = _dto.Limit,
        UserId = TestFactoryUsers.DefaultUser1Authenticated.Id,
    };

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}