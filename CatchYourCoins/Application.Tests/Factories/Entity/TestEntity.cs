using System;
using Domain;
using Domain.Interfaces.Repositories;

namespace Application.Tests.Factories.Entity;

public class TestEntity : IEntity, IAutorizable
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required Guid UserId { get; init; }
}