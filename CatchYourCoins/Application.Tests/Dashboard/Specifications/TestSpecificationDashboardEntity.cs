using System;
using Application.Tests.TestObjects.Entity;
using Application.Tests.TestObjects.Specification;
using Domain.Dashboard.Specifications;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Dashboard.Specifications;

[TestSubject(typeof(SpecificationDashboardEntity<,>))]
public class TestSpecificationDashboardEntity
{
    #region Id

    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(1, 2, false)]
    public void Build_WithValidId_CriteriaIsCorrect(
        int searchedId,
        int checkedId,
        bool expectedResult)
    {
        // Arrange
        var specification = TestObjSpecificationDashboardEntity.GetBuilder()
            .WithId(searchedId)
            .Build();

        // Act
        Func<TestObjDashboardEntity, bool> compiledFunc = specification.Criteria.Compile();

        // Assert
        compiledFunc(new TestObjDashboardEntity
        {
            Id = checkedId,
            UserId = Guid.Empty,
        }).Should().Be(expectedResult);
    }
    
    [Fact]
    public void Build_WithInvalidId_ThrowsException()
    {
        // Arrange
        const int id = -1;
        var builder = TestObjSpecificationDashboardEntity.GetBuilder();
        
        // Act
        Action act = () => builder.WithId(id);
        
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, 1, true)]
    [InlineData(new[] { 1, 2 }, 2, true)]
    [InlineData(new[] { 1, 2 }, 3, false)]
    public void Build_WithValidIdRange_CriteriaIsCorrect(
        int[] searchedIds,
        int checkedId,
        bool expectedResult)
    {
        // Arrange
        var specification = TestObjSpecificationDashboardEntity.GetBuilder()
            .WithIdRange(searchedIds)
            .Build();

        // Act
        Func<TestObjDashboardEntity, bool> compiledFunc = specification.Criteria.Compile();

        // Assert
        compiledFunc(new TestObjDashboardEntity
        {
            Id = checkedId,
            UserId = Guid.Empty,
        }).Should().Be(expectedResult);
    }
    
    [Theory]
    [InlineData(new[] { -1, 2 })]
    [InlineData(new[] { 1, -2 })]
    public void Build_WithInvalidIdRange_ThrowsException(int[] ids)
    {
        // Arrange
        var builder = TestObjSpecificationDashboardEntity.GetBuilder();
        
        // Act
        Action act = () => builder.WithIdRange(ids);
        
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion
}