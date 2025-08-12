using System;
using System.Linq.Expressions;
using Application.Tests.TestObjects.Entity;
using Domain.Dashboard.Specifications;

namespace Application.Tests.TestObjects.Specification;

public class TestObjSpecificationFinancialOperation : SpecificationFinancialOperation<
        TestObjFinancialOperation,
        TestObjSpecificationFinancialOperation.TestObjBuilder,
        TestObjCategory
    >
{
    private TestObjSpecificationFinancialOperation(Expression<Func<TestObjFinancialOperation, bool>> criteria) : base(criteria) {}

    public static TestObjBuilder GetBuilder() => new();

    public class TestObjBuilder : BuilderFinancialOperation
    {
        internal TestObjBuilder() {}
        public override TestObjSpecificationFinancialOperation Build() => new(criteria);
    }
}