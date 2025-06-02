using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

/// <summary>
/// Provides methods for generating test data for DeleteUserHandler.
/// </summary>
public static class DeleteUserHandlerTestData
{
    private static readonly Faker<DeleteUserCommand> deleteUserHandlerFaker = new Faker<DeleteUserCommand>("pt_BR")
        .CustomInstantiator(f => new DeleteUserCommand(f.Random.Guid()));

    /// <summary>
    /// Generates a valid command for user deletion.
    /// </summary>
    public static DeleteUserCommand GenerateValidCommand()
    {
        return deleteUserHandlerFaker.Generate();
    }
}