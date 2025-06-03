using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for the Product entity.
/// </summary>
public static class ProductTestData
{
    private static readonly Faker<Product> ProductFaker = new Faker<Product>("pt_BR")
        .RuleFor(p => p.Title, f => f.Commerce.ProductName())
        .RuleFor(p => p.Price, f => f.Random.Decimal(0, 10000))
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
        .RuleFor(p => p.Rating, f => new ProductRating(
            f.Random.Decimal(0, 5),
            f.Random.Number(0, 1000)))
        .RuleFor(p => p.CreatedAt, f => f.Date.Past().ToUniversalTime())
        .RuleFor(p => p.UpdatedAt, f => f.Date.Recent().ToUniversalTime());

    /// <summary>
    /// Generates a valid Product entity with randomized data.
    /// </summary>
    public static Product GenerateValidProduct()
    {
        return ProductFaker.Generate();
    }

    /// <summary>
    /// Generates an invalid Product entity for negative test scenarios.
    /// </summary>
    public static Product GenerateInvalidProduct()
    {
        return new Product
        {
            Title = "", // Invalid: empty
            Price = -10, // Invalid: negative
            Description = new string('a', 1001), // Invalid: > 1000 characters
            Category = "", // Invalid: empty
            Image = "", // Invalid: empty
            Rating = new ProductRating(6, -1), // Invalid: rate > 5, count < 0
            CreatedAt = DateTime.UtcNow
        };
    }
}