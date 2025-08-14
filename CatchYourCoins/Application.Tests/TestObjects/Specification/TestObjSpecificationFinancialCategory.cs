using System;
using System.Linq.Expressions;
using Application.Tests.TestObjects.Entity;
using Domain.Dashboard.Specifications;

namespace Application.Tests.TestObjects.Specification;

public class TestObjSpecificationFinancialCategory : SpecificationFinancialCategory<
        TestObjFinancialCategory,
        TestObjSpecificationFinancialCategory.TestObjBuilder>
{
    private TestObjSpecificationFinancialCategory(Expression<Func<TestObjFinancialCategory, bool>> criteria) : base(criteria) {}

    public static TestObjBuilder GetBuilder() => new();

    public class TestObjBuilder : BuilderFinancialCategory
    {
        internal TestObjBuilder() {}
        public override TestObjSpecificationFinancialCategory Build() => new(criteria);
    }
}