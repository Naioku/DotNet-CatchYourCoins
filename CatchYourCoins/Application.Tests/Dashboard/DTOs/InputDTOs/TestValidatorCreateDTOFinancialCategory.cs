using Application.Dashboard.DTOs.CreateDTOs;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Dashboard.DTOs.InputDTOs;

[TestSubject(typeof(ValidatorCreateDTOFinancialCategory<>))]
public class TestValidatorCreateDTOFinancialCategory
    : TestValidatorBase<ValidatorCreateDTOFinancialCategory<TestCreateDTOFinancialCategory>,
        TestCreateDTOFinancialCategory
    >
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new TestCreateDTOFinancialCategory
        {
            Name = "Test",
            Limit = 1000,
        });

    [Fact]
    public void Validate_MinimalValidData_NoError() =>
        AssertSuccess(new TestCreateDTOFinancialCategory
        {
            Name = "Test",
        });
    
    [Fact]
    public void Validate_EmptyName_Error() =>
        AssertFailure(new TestCreateDTOFinancialCategory
        {
            Name = "",
        });
    
    [Fact]
    public void Validate_NullName_Error() =>
        AssertFailure(new TestCreateDTOFinancialCategory
        {
            Name = null,
        });
}