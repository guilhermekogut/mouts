namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

/// <summary>
/// Represents the name details of a user.
/// </summary>
public class NameRequest
{
    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string Firstname { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string Lastname { get; set; } = string.Empty;
}
