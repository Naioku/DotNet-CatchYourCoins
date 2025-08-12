using System.Collections.Generic;
using Application.Tests.TestObjects.Entity;
using Domain.Dashboard.Entities;

namespace Application.Tests.Factories.Entity;

public class TestFactoryEntity
{
    public TestObjEntity CreateEntity(CurrentUser currentUser, int id = 1, string namePrefix = "Test") => new()
    {
        Id = id,
        Name = $"{namePrefix} {id}",
        UserId = currentUser.Id,
    };
    
    public List<TestObjEntity> CreateEntities(CurrentUser currentUser, int quantity, string namePrefix = "Test")
    {
        List<TestObjEntity> result = [];
        for (int i = 0; i < quantity; i++)
        {
            result.Add(CreateEntity(currentUser, i + 1, namePrefix));
        }
        
        return result;
    }
}