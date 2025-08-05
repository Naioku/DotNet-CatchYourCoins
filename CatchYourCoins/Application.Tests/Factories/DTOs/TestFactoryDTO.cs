using Application.Tests.Factories.Entity;

namespace Application.Tests.Factories.DTOs;

public class TestFactoryDTO : TestFactoryDTOBase<TestEntity, TestDTO>
{
    public override TestDTO CreateDTO(TestEntity entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
}