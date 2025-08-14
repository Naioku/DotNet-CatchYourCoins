using System;
using Domain.Dashboard.Entities.Expenses;
using Domain.Dashboard.Specifications.Expenses;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Dashboard.Specifications.Expenses;

[TestSubject(typeof(SpecificationExpense))]
public partial class TestSpecificationExpense
{
    [Theory]
    [MemberData(nameof(TestCasesPaymentMethodId))]
    public void Build_WithValidPaymentMethodId_CriteriaIsCorrect(TestCasePaymentMethodId testCase)
    {
        // Arrange
        var specification = SpecificationExpense.GetBuilder()
            .WithPaymentMethod(testCase.SearchedPaymentMethodId)
            .Build();
        
        // Act
        Func<Expense, bool> compiledFunc = specification.Criteria.Compile();
        
        // Assert
        compiledFunc(new Expense
        {
            Amount = 0,
            UserId = Guid.Empty,
            Date = default,
            PaymentMethodId = testCase.CheckedPaymentMethod?.Id,
            PaymentMethod = testCase.CheckedPaymentMethod,
        }).Should().Be(testCase.ExpectedResult);
    }
    
    [Fact]
    public void Build_WithInvalidPaymentMethodId_ThrowsException()
    {
        const int id = -1;
        // Arrange
        var builder = SpecificationExpense.GetBuilder();
        
        // Act
        Action act = () => builder.WithPaymentMethod(id);
        
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [MemberData(nameof(TestCasesPaymentMethodName))]
    public void Build_WithValidPaymentMethodName_CriteriaIsCorrect(TestCasePaymentMethodName testCase)
    {
        // Arrange
        var specification = SpecificationExpense.GetBuilder()
            .WithPaymentMethod(testCase.SearchedPaymentMethodName)
            .Build();
        
        // Act
        Func<Expense, bool> compiledFunc = specification.Criteria.Compile();
        
        // Assert
        compiledFunc(new Expense
        {
            Amount = 0,
            UserId = Guid.Empty,
            Date = default,
            PaymentMethod = testCase.CheckedPaymentMethod,
        }).Should().Be(testCase.ExpectedResult);
    }
}