using System.Collections.Generic;
using System.Linq;
using Application.Tests.Factories.Entity;

namespace Application.Tests.Factories.DTOs;

public class TestFactoryDTO
{
    public TestDTO CreateDTO(TestEntity entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
        };

    public TestDTO CreateDTO(int id = 1, string name = "Test") =>
        new()
        {
            Id = id,
            Name = name,
        };

    public List<TestDTO> CreateDTOs(IEnumerable<TestEntity> entity) => entity.Select(CreateDTO).ToList();

    public List<TestDTO> CreateDTOs(int quantity, string namePrefix = "Test")
    {
        List<TestDTO> result = [];
        for (int i = 0; i < quantity; i++)
        {
            result.Add(CreateDTO(i + 1, $"{namePrefix} {i + 1}"));
        }

        return result;
    }
}