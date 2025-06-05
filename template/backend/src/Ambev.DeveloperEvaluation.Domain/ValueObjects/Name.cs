namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;
public class Name
{
    public Name() { }

    public Name(string firstname, string lastname)
    {
        Firstname = firstname;
        Lastname = lastname;
    }
    /// <summary>
    /// Gets the firstname
    /// </summary>
    public string Firstname { get; set; } = string.Empty;
    /// <summary>
    /// Gets the lastname
    /// </summary>
    public string Lastname { get; set; } = string.Empty;
}
