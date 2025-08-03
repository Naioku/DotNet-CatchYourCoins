using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Incomes;
using Application.Requests.Incomes.Commands.Create;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Incomes.Commands.Create.CreateCategory;

[TestSubject(typeof(HandlerCreateCategory))]
public class HandlerCreateCategoryTest
    : TestHandlerCreate<
        HandlerCreateCategory,
        IncomeCategory,
        InputDTOIncomeCategory,
        CommandCreateCategory,
        IRepositoryIncomeCategory
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
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override CommandCreateCategory GetCommand(InputDTOIncomeCategory dto) => new() { Data = dto };

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
    
    [Fact]
    public async Task Create_RepositoryThrowsException_EntityNotCreated() =>
        await Create_RepositoryThrowsException_EntityNotCreated_Base();
    
    [Fact]
    public async Task Create_UnitOfWorkThrowsException_EntityNotCreated() =>
        await Create_UnitOfWorkThrowsException_EntityNotCreated_Base();
}