using System;
using System.Diagnostics.CodeAnalysis;
using Application.Tests.TestObjects.Entity;
using Application.Tests.TestObjects.Specification;
using Domain.Dashboard.Specifications;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Dashboard.Specifications;

[TestSubject(typeof(SpecificationFinancialOperation<,,>))]
public partial class TestSpecificationFinancialOperation
{
    #region Amount

    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(1, 2, false)]
    public void Build_WithValidAmount_CriteriaIsCorrect(
        decimal searchedAmount,
        decimal checkedAmount,
        bool searchedResult)
    {
        // Arrange
        var specification = TestObjSpecificationFinancialOperation.GetBuilder()
            .WithAmount(searchedAmount)
            .Build();
        
        // Act
        Func<TestObjFinancialOperation, bool> compiledFunc = specification.Criteria.Compile();
        
        // Assert
        compiledFunc(new TestObjFinancialOperation
        {
            Amount = checkedAmount,
            UserId = Guid.Empty,
            Date = default,
        }).Should().Be(searchedResult);
    }

    [Fact]
    public void Build_WithInvalidAmount_ThrowsException()
    {
        // Arrange
        const decimal amount = -1;
        var builder = TestObjSpecificationFinancialOperation.GetBuilder();
        
        // Act
        Action act = () => builder.WithAmount(amount);
        
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
        decimal searchedAmountRangeMin,
        decimal searchedAmountRangeMax,
        decimal checkedAmount,
        bool expectedResult)
    {
        // Arrange
        var specification = TestObjSpecificationFinancialOperation.GetBuilder()
            .WithAmountRange(searchedAmountRangeMin, searchedAmountRangeMax)
            .Build();
        
        // Act
        Func<TestObjFinancialOperation, bool> compiledFunc = specification.Criteria.Compile();
        
        // Assert
        compiledFunc(new TestObjFinancialOperation
        {
            Amount = checkedAmount,
            UserId = Guid.Empty,
            Date = default,
        }).Should().Be(expectedResult);
    }
    
    [Theory]
    [MemberData(nameof(TestCasesAmountRangeException))]
    public void Build_WithInvalidAmountRange_ThrowsException(TestCaseAmountRangeException testCase)
    {
        // Arrange
        var builder = TestObjSpecificationFinancialOperation.GetBuilder();
        
        // Act
        Action act = () => builder.WithAmountRange(testCase.AmountMin, testCase.AmountMax);
        
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion
    
    #region Date

    [Theory]
    [InlineData("2021-01-01", "2021-01-01", true)]
    [InlineData("2021-01-01", "2022-02-02", false)]
    public void Build_WithValidDate_CriteriaIsCorrect(
        string searchedDateString,
        string checkedDateString,
        bool expectedResult)
    {
        // Arrange
        DateTime searchedDate = DateTime.Parse(searchedDateString);
        DateTime checkedDate = DateTime.Parse(checkedDateString);
        var specification = TestObjSpecificationFinancialOperation.GetBuilder()
            .WithDate(searchedDate)
            .Build();
        
        // Act
        Func<TestObjFinancialOperation, bool> compiledFunc = specification.Criteria.Compile();
        
        // Assert
        compiledFunc(new TestObjFinancialOperation
        {
            Amount = 0,
            UserId = Guid.Empty,
            Date = checkedDate,
        }).Should().Be(expectedResult);
    }
    
    [Theory]
    [InlineData("2021-01-01", "2021-01-03", "2021-01-01", true)]
    [InlineData("2021-01-01", "2021-01-03", "2021-01-03", true)]
    [InlineData("2021-01-01", "2021-01-03", "2020-01-03", false)]
    [InlineData("2021-01-01", "2021-01-03", "2022-01-03", false)]
    public void Build_WithValidDateRange_CriteriaIsCorrect(
        string searchedDateMinString,
        string searchedDateMaxString,
        string checkedDateString,
        bool expectedResult)
    {
        // Arrange
        DateTime searchedDateMin = DateTime.Parse(searchedDateMinString);
        DateTime searchedDateMax = DateTime.Parse(searchedDateMaxString);
        DateTime checkedDate = DateTime.Parse(checkedDateString);
        var specification = TestObjSpecificationFinancialOperation.GetBuilder()
            .WithDateRange(searchedDateMin, searchedDateMax)
            .Build();
        
        // Act
        Func<TestObjFinancialOperation, bool> compiledFunc = specification.Criteria.Compile();
        
        // Assert
        compiledFunc(new TestObjFinancialOperation
        {
            Amount = 0,
            UserId = Guid.Empty,
            Date = checkedDate,
        }).Should().Be(expectedResult);
    }
    
    [Theory]
    [InlineData("2021-01-09", "2021-01-01", "Min > Max")]
    [SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
    public void Build_WithInvalidDateRange_ThrowsException(
        string searchedDateMinString,
        string searchedDateMaxString,
        string testCase)
    {
        // Arrange
        DateTime searchedDateMin = DateTime.Parse(searchedDateMinString);
        DateTime searchedDateMax = DateTime.Parse(searchedDateMaxString);
        var builder = TestObjSpecificationFinancialOperation.GetBuilder();
        
        // Act
        Action act = () => builder.WithDateRange(searchedDateMin, searchedDateMax);
        
        // Assert
        act.Should().Throw<ArgumentException>();
    }
    
    #endregion

    #region Description

    [Theory]
    [InlineData("Test1", "Test1", true)]
    [InlineData(null, null, true)]
    [InlineData("", null, true)]
    [InlineData("Test1", "Test2", false)]
    [InlineData(null, "Test2", false)]
    [InlineData("Test1", null, false)]
    public void Build_WithValidDescription_CriteriaIsCorrect(
        string? searchedDescription,
        string? checkedDescription,
        bool expectedResult)
    {
        // Arrange
        var specification = TestObjSpecificationFinancialOperation.GetBuilder()
            .WithDescription(searchedDescription)
            .Build();
        
        // Act
        Func<TestObjFinancialOperation, bool> compiledFunc = specification.Criteria.Compile();
        
        // Assert
        compiledFunc(new TestObjFinancialOperation
        {
            Amount = 0,
            UserId = Guid.Empty,
            Date = default,
            Description = checkedDescription,
        }).Should().Be(expectedResult);
    }

    #endregion
    
    #region Category
    
    [Theory]
    [MemberData(nameof(TestCasesCategoryId))]
    public void Build_WithValidCategoryId_CriteriaIsCorrect(TestCaseCategoryId testCase)
    {
        // Arrange
        var specification = TestObjSpecificationFinancialOperation.GetBuilder()
            .WithCategory(testCase.SearchedCategoryId)
            .Build();
        
        // Act
        Func<TestObjFinancialOperation, bool> compiledFunc = specification.Criteria.Compile();
        
        // Assert
        compiledFunc(new TestObjFinancialOperation
        {
            Amount = 0,
            UserId = Guid.Empty,
            Date = default,
            CategoryId = testCase.CheckedCategory?.Id,
            Category = testCase.CheckedCategory,
        }).Should().Be(testCase.ExpectedResult);
    }
    
    [Fact]
    public void Build_WithInvalidCategoryId_ThrowsException()
    {
        const int id = -1;
        // Arrange
        var builder = TestObjSpecificationFinancialOperation.GetBuilder();
        
        // Act
        Action act = () => builder.WithCategory(id);
        
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [MemberData(nameof(TestCasesCategoryName))]
    public void Build_WithValidCategoryName_CriteriaIsCorrect(TestCaseCategoryName testCase)
    {
        // Arrange
        var specification = TestObjSpecificationFinancialOperation.GetBuilder()
            .WithCategory(testCase.SearchedCategoryName)
            .Build();
        
        // Act
        Func<TestObjFinancialOperation, bool> compiledFunc = specification.Criteria.Compile();
        
        // Assert
        compiledFunc(new TestObjFinancialOperation
        {
            Amount = 0,
            UserId = Guid.Empty,
            Date = default,
            Category = testCase.CheckedCategory,
        }).Should().Be(testCase.ExpectedResult);
    }

    #endregion
}