using Ambev.DeveloperEvaluation.Domain.Validation;

using FluentAssertions;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation
{
    /// <summary>
    /// Contains unit tests for the <see cref="ZipCodeValidator"/> class.
    /// Tests validation of zipcodes according to the following rules:
    /// - Must not be empty
    /// - Must match pattern: ^\d{5}-\d{3}$
    /// </summary>
    public class ZipCodeValidatorTests
    {
        [Theory(DisplayName = "Given a phone number When validating Then should validate according to regex pattern")]
        [InlineData("80000-100", true)]      // Valid
        [InlineData("80000000", false)]    // Invalid - no hyphen
        [InlineData("", false)]               // Invalid - empty
        public void Given_ZipCode_When_Validating_Then_ShouldValidateAccordingToPattern(string zipCode, bool expectedResult)
        {
            // Arrange
            var validator = new ZipCodeValidator();

            // Act
            var result = validator.Validate(zipCode);

            // Assert
            result.IsValid.Should().Be(expectedResult);
        }
    }
}
