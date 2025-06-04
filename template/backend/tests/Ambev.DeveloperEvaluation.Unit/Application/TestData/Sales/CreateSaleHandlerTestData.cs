using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

/// <summary>
/// Provides methods for generating test data for CreateSaleHandler unit tests.
/// </summary>
public static class CreateSaleHandlerTestData
{
    private static readonly Faker<CreateSaleCommand> CommandFaker = new Faker<CreateSaleCommand>("pt_BR")
        .RuleFor(c => c.CartId, f => f.Random.Guid())
        .RuleFor(c => c.AuthenticatedUserId, f => f.Random.Guid());

    public static CreateSaleCommand GenerateValidCommand() => CommandFaker.Generate();

    public static CreateSaleCommand GenerateCommandWithInvalidCartId()
    {
        var command = GenerateValidCommand();
        command.CartId = Guid.Empty;
        return command;
    }

    public static CreateSaleCommand GenerateCommandWithInvalidUser()
    {
        var command = GenerateValidCommand();
        command.AuthenticatedUserId = Guid.NewGuid();
        return command;
    }
}