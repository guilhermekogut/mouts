using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Represents the response returned after successfully creating a new user.
/// </summary>
/// <remarks>
/// This response contains the unique identifier of the newly updated user,
/// which can be used for subsequent operations or reference.
/// </remarks>
public class UpdateUserResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the newly updated user.
    /// </summary>
    /// <value>A GUID that uniquely identifies the updated user in the system.</value>
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public UserStatus Status { get; set; }
    public UserRole Role { get; set; }
    public NameResult Name { get; set; } = new();
    public AddressResult Address { get; set; } = new();
}
