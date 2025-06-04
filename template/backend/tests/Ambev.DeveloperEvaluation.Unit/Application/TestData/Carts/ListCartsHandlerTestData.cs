using Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Carts;

/// <summary>
/// Provides methods for generating test data for ListCartsHandler unit tests.
/// </summary>
public static class ListCartsHandlerTestData
{
    private static readonly Faker<ListCartsCommand> Faker = new Faker<ListCartsCommand>("pt_BR")
        .RuleFor(c => c.Page, 1)
        .RuleFor(c => c.Size, 10);

    public static ListCartsCommand GenerateValidCommand() => Faker.Generate();

    public static ListCartsCommand GenerateInvalidCommand() => new ListCartsCommand
    {
        Page = 0,
        Size = 0
    };
}