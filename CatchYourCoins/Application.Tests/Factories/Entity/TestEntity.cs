using System;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;

namespace Application.Tests.Factories.Entity;

public class TestEntity : DashboardEntity
{
    public required string Name { get; init; }
}