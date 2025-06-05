using Ambev.DeveloperEvaluation.Domain.Validation;

using FluentAssertions;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation
{
    /// <summary>
    /// Contains unit tests for the UrlValidator class.
    /// </summary>
    public class UrlValidatorTests
    {
        private readonly UrlValidator _validator = new UrlValidator();

        [Theory(DisplayName = "Given valid URLs When validating Then validation should succeed")]
        [InlineData("http://example.com")]
        [InlineData("https://example.com")]
        [InlineData("https://sub.domain.com/path?query=1#fragment")]
        [InlineData("https://example.com:8080/path")]
        public void Given_ValidUrls_When_Validated_Then_ShouldBeValid(string url)
        {
            // Act
            var result = _validator.Validate(url);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory(DisplayName = "Given invalid URLs When validating Then validation should fail")]
        [InlineData("")]
        [InlineData("not a url")]
        [InlineData("ftp://example.com")]
        [InlineData("http:/example.com")]
        [InlineData("http://")]
        [InlineData("https://")]
        [InlineData("http://.com")]
        [InlineData("http://localhost")]
        [InlineData("http://example")]
        public void Given_InvalidUrls_When_Validated_Then_ShouldBeInvalid(string url)
        {
            // Act
            var result = _validator.Validate(url);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}