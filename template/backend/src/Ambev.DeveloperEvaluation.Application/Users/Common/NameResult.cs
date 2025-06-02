namespace Ambev.DeveloperEvaluation.Application.Users.Common;

public class NameResult
{

    public NameResult() { }

    public NameResult(string firstname, string lastname)
    {
        Firstname = firstname;
        Lastname = lastname;
    }

    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
}
