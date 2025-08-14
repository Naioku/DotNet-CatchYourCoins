using System;
using System.Linq.Expressions;
using Application.Tests.TestObjects.Entity;
using Domain.Dashboard.Specifications;

namespace Application.Tests.TestObjects.Specification;

public class TestObjSpecificationDashboardEntity : SpecificationDashboardEntity<
    TestObjDashboardEntity,
    TestObjSpecificationDashboardEntity.TestObjBuilder>
{
    private TestObjSpecificationDashboardEntity(Expression<Func<TestObjDashboardEntity, bool>> criteria) : base(criteria) {}

    public static TestObjBuilder GetBuilder() => new();

    public class TestObjBuilder : BuilderDashboardEntity
    {
        internal TestObjBuilder() {}
        public override TestObjSpecificationDashboardEntity Build() => new(criteria);
    }
}