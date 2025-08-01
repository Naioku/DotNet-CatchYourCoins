using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Incomes;
using Application.Incomes.Commands.Create;
using Application.Tests.Factories;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Commands.Create.CreateCategory;

[TestSubject(typeof(HandlerCreateCategory))]
public class HandlerCreateCategoryTest
    : TestHandlerCreate<
        HandlerCreateCategory,
        IncomeCategory,
        InputDTOIncomeCategory,
        CommandCreateCategory,
        IRepositoryIncomeCategory,
        TestFactoryIncomeCategory
    >
{
    private readonly InputDTOIncomeCategory _dto = new()
    {
        Name = "Test1",
        Limit = 1000
    };
    
    protected override HandlerCreateCategory CreateHandler()
    {
        return new HandlerCreateCategory(
            GetMock<IRepositoryIncomeCategory>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override InputDTOIncomeCategory GetInputDTO() => _dto;

    protected override CommandCreateCategory GetCommand() => new() { Data = _dto };

    protected override IncomeCategory GetMappedEntity() => new()
    {
        Name = _dto.Name,
        Limit = _dto.Limit,
        UserId = TestFactoryUsers.DefaultUser1Authenticated.Id,
    };

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}