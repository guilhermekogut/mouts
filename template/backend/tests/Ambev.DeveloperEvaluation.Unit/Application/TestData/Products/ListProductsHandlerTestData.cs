using Ambev.DeveloperEvaluation.Application.Products.ListProducts;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;

/// <summary>
/// Provides methods for generating test data for ListProductsHandler unit tests.
/// </summary>
public static class ListProductsHandlerTestData
{
    private static readonly Faker<ListProductsCommand> Faker = new Faker<ListProductsCommand>("pt_BR")
        .RuleFor(c => c.Page, 1)
        .RuleFor(c => c.Size, 10);

    public static ListProductsCommand GenerateValidCommand() => Faker.Generate();

    public static ListProductsCommand GenerateInvalidCommand() => new ListProductsCommand
    {
        Page = 0,
        Size = 0
    };
}