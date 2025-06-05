using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;

/// <summary>
/// Provides methods for generating test data for CreateProductHandler unit tests.
/// </summary>
public static class CreateProductHandlerTestData
{
    private static readonly Faker<CreateProductCommand> Faker = new Faker<CreateProductCommand>("pt_BR")
        .RuleFor(c => c.Title, f => f.Commerce.ProductName())
        .RuleFor(c => c.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
        .RuleFor(c => c.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(c => c.Image, f => f.Image.PicsumUrl());

    public static CreateProductCommand GenerateValidCommand() => Faker.Generate();

    public static CreateProductCommand GenerateInvalidCommand() => new CreateProductCommand
    {
        Title = "",
        Price = -1,
        Description = "",
        Category = "",
        Image = ""
    };
}