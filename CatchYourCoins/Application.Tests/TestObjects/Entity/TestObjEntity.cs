using Domain.Dashboard.Entities;

namespace Application.Tests.TestObjects.Entity;

public class TestObjEntity : DashboardEntity
{
    public required string Name { get; init; }
}