using System;
using Application.Tests.TestObjects.Entity;
using Application.Tests.TestObjects.Specification;
using Domain.Dashboard.Specifications;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Dashboard.Specifications;

[TestSubject(typeof(SpecificationFinancialCategory<,>))]
public partial class TestSpecificationFinancialCategory
{
    #region Name

    [Theory]
    [InlineData("Test1", "Test1", true)]
    [InlineData("Test", "Test1", true)]
    [InlineData("TesT", "Test1", true)]
    [InlineData("Test1", "Test2", false)]
    public void Build_WithValidName_CriteriaIsCorrect(
        string searchedName,
        string checkedName,
        bool expectedResult)
    {
        // Arrange
        var specification = TestObjSpecificationFinancialCategory.GetBuilder()
            .WithName(searchedName)
            .Build();
        
        // Act
        Func<TestObjFinancialCategory, bool> compiledFunc = specification.Criteria.Compile();
        
        // Assert
        compiledFunc(new TestObjFinancialCategory
        {
            Name = checkedName,
            UserId = Guid.Empty,
        }).Should().Be(expectedResult);
    }

    #endregion

    #region Limit

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Build_WithInvalidName_ThrowsException(string? searchedName)
    {
        // Arrange
        var builder = TestObjSpecificationFinancialCategory.GetBuilder();
        
        // Act
        Action act = () => builder.WithName(searchedName);
        
        // Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(1, 2, false)]
    public void Build_WithValidLimit_CriteriaIsCorrect(
        decimal searchedLimit,
        decimal checkedLimit,
        bool expectedResult)
    {
        // Arrange
        var specification = TestObjSpecificationFinancialCategory.GetBuilder()
            .WithLimit(searchedLimit)
            .Build();
        
        // Act
        Func<TestObjFinancialCategory, bool> compiledFunc = specification.Criteria.Compile();
        
        // Assert
        compiledFunc(new TestObjFinancialCategory
        {
            Name = "Test",
            Limit = checkedLimit,
            UserId = Guid.Empty,
        }).Should().Be(expectedResult);
    }
    
    [Fact]
    public void Build_WithInvalidLimit_ThrowsException()
    {
        // Arrange
        const decimal limit = -1;
        var builder = TestObjSpecificationFinancialCategory.GetBuilder();
        
        // Act
        Action act = () => builder.WithLimit(limit);
        
        // Assert
        act.Should().Throw<ArgumentException>();
    }
    
    [Theory]
    [InlineData(5, 10, 5, true)]
    [InlineData(5, 10, 7, true)]
    [InlineData(5, 10, 10, true)]
    [InlineData(5, 10, 4, false)]
    [InlineData(5, 10, 11, false)]
    public void Build_WithValidAmountRange_CriteriaIsCorrect(
        decimal searchedLimitRangeMin,
        decimal searchedLimitRangeMax,
        decimal checkedLimit,
        bool expectedResult)
    {
        // Arrange
        var specification = TestObjSpecificationFinancialCategory.GetBuilder()
            .WithLimitRange(searchedLimitRangeMin, searchedLimitRangeMax)
            .Build();
        
        // Act
        Func<TestObjFinancialCategory, bool> compiledFunc = specification.Criteria.Compile();
        
        // Assert
        compiledFunc(new TestObjFinancialCategory
        {
            Name = "Test",
            Limit = checkedLimit,
            UserId = Guid.Empty,
        }).Should().Be(expectedResult);
    }
    
    [Theory]
    [MemberData(nameof(TestCasesAmountRangeException))]
    public void Build_WithInvalidAmountRange_ThrowsException(TestCaseAmountRangeException testCase)
    {
        // Arrange
        var builder = TestObjSpecificationFinancialCategory.GetBuilder();
        
        // Act
        Action act = () => builder.WithLimitRange(testCase.AmountMin, testCase.AmountMax);
        
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion
}