using System;
using System.Collections.Generic;
using Application.Tests.Factories.DTOs;
using Application.Tests.Factories.Entity;

namespace Application.Tests.Factories;

public static class TestFactoriesProvider
{
    private static readonly Dictionary<Type, object> Factories = new();
    
    public static T GetFactory<T>() => (T)Factories[typeof(T)];
    
    static TestFactoriesProvider()
    {
        Factories.Add(typeof(TestFactoryEntityBase<TestEntity>), new TestFactoryEntity());
        Factories.Add(typeof(TestFactoryDTOBase<TestEntity, TestDTO>), new TestFactoryDTO());
        Factories.Add(typeof(TestFactoryUsers), new TestFactoryUsers());
    }
}