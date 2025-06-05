using System.Text.RegularExpressions;

using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for URL format, to be used with FluentValidation.
/// </summary>
public class UrlValidator : AbstractValidator<string>
{
    public UrlValidator()
    {
        RuleFor(url => url)
            .NotEmpty()
            .WithMessage("The URL cannot be empty.")
            .MaximumLength(300)
            .WithMessage("The URL cannot be longer than 300 characters.")
            .Must(BeValidUrl)
            .WithMessage("The provided URL is not valid.");
    }

    private bool BeValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        // Basic URL validation (http/https, domain, optional path/query)
        var regex = new Regex(@"^(https?:\/\/)" + // protocol
                              @"([\w\-]+\.)+[\w\-]+" + // domain
                              @"([\/\w\-.~:?#[\]@!$&'()*+,;=]*)?$", // path/query/fragment
                              RegexOptions.IgnoreCase);
        return regex.IsMatch(url);
    }
}