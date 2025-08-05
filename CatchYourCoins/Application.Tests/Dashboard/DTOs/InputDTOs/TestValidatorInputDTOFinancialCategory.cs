using Application.Dashboard.DTOs.InputDTOs;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Dashboard.DTOs.InputDTOs;

[TestSubject(typeof(ValidatorInputDTOFinancialCategory<>))]
public class TestValidatorInputDTOFinancialCategory
    : TestValidatorBase<ValidatorInputDTOFinancialCategory<InputDTOFinancialCategory>,
        InputDTOFinancialCategory
    >
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new InputDTOFinancialCategory
        {
            Name = "Test",
            Limit = 1000,
        });

    [Fact]
    public void Validate_MinimalValidData_NoError() =>
        AssertSuccess(new InputDTOFinancialCategory
        {
            Name = "Test",
        });
    
    [Fact]
    public void Validate_EmptyName_Error() =>
        AssertFailure(new InputDTOFinancialCategory
        {
            Name = "",
        });
}