using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;

/// <summary>
/// Provides methods for generating test data for UpdateProductHandler unit tests.
/// </summary>
public static class UpdateProductHandlerTestData
{
    private static readonly Faker<UpdateProductCommand> Faker = new Faker<UpdateProductCommand>("pt_BR")
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(c => c.Title, f => f.Commerce.ProductName())
        .RuleFor(c => c.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
        .RuleFor(c => c.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(c => c.Image, f => f.Image.PicsumUrl());

    public static UpdateProductCommand GenerateValidCommand() => Faker.Generate();

    public static UpdateProductCommand GenerateInvalidCommand() => new UpdateProductCommand
    {
        Id = Guid.Empty,
        Title = "",
        Price = -1,
        Description = "",
        Category = "",
        Image = ""
    };
}