using System.Collections.Generic;
using System.Linq;
using Application.Tests.TestObjects;
using Application.Tests.TestObjects.Entity;

namespace Application.Tests.Factories.DTOs;

public class TestFactoryDTO
{
    public TestObjDTO CreateDTO(TestObjEntity entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
        };

    public TestObjDTO CreateDTO(int id = 1, string name = "Test") =>
        new()
        {
            Id = id,
            Name = name,
        };

    public List<TestObjDTO> CreateDTOs(IEnumerable<TestObjEntity> entity) => entity.Select(CreateDTO).ToList();

    public List<TestObjDTO> CreateDTOs(int quantity, string namePrefix = "Test")
    {
        List<TestObjDTO> result = [];
        for (int i = 0; i < quantity; i++)
        {
            result.Add(CreateDTO(i + 1, $"{namePrefix} {i + 1}"));
        }

        return result;
    }
}