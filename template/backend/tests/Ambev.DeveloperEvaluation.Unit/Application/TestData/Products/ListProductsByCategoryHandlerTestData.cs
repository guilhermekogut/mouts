using Ambev.DeveloperEvaluation.Application.Products.ListProductsByCategory;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;

/// <summary>
/// Provides methods for generating test data for ListProductsByCategoryHandler unit tests.
/// </summary>
public static class ListProductsByCategoryHandlerTestData
{
    private static readonly Faker<ListProductsByCategoryCommand> Faker = new Faker<ListProductsByCategoryCommand>("pt_BR")
        .RuleFor(c => c.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(c => c.Page, 1)
        .RuleFor(c => c.Size, 10);

    public static ListProductsByCategoryCommand GenerateValidCommand() => Faker.Generate();

    public static ListProductsByCategoryCommand GenerateInvalidCommand() => new ListProductsByCategoryCommand
    {
        Category = "",
        Page = 0,
        Size = 0
    };
}