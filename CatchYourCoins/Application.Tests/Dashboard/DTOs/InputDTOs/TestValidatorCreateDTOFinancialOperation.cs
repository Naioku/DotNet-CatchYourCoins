using System;
using Application.Dashboard.DTOs.CreateDTOs;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Dashboard.DTOs.InputDTOs;

[TestSubject(typeof(ValidatorCreateDTOFinancialOperation<>))]
public class TestValidatorCreateDTOFinancialOperation
    : TestValidatorBase<
        ValidatorCreateDTOFinancialOperation<TestCreateDTOFinancialOperation>,
        TestCreateDTOFinancialOperation
    >
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new TestCreateDTOFinancialOperation
        {
            Amount = 1000,
            Date = DateTime.Now,
            Description = "Test",
            CategoryId = 1,
        });

    [Fact]
    public void Validate_MinimalValidData_NoError() =>
        AssertSuccess(new TestCreateDTOFinancialOperation
        {
            Amount = 1000,
            Date = DateTime.Now,
        });

    [Fact]
    public void Validate_AmountGreaterThanMaxValue_Error()
    {
        AssertFailure(new TestCreateDTOFinancialOperation
        {
            Amount = 9999999999999999.99m + .01m,
            Date = DateTime.Now,
        });
    }

    [Fact]
    public void Validate_AmountLessThan0_Error() =>
        AssertFailure(new TestCreateDTOFinancialOperation
        {
            Amount = -1000,
            Date = DateTime.Now,
        });
    
    [Fact]
    public void Validate_DescriptionPassedAndIsTooShort_Error() =>
        AssertFailure(new TestCreateDTOFinancialOperation
        {
            Amount = -1000,
            Date = DateTime.Now,
            Description = "",
        });
    
    [Fact]
    public void Validate_DescriptionPassedAndIsTooLong_Error() =>
        AssertFailure(new TestCreateDTOFinancialOperation
        {
            Amount = -1000,
            Date = DateTime.Now,
            Description = MultiplyCharacter("a", 8001),
        });

    private static string MultiplyCharacter(string str, int count) => new(str[0], count);
}