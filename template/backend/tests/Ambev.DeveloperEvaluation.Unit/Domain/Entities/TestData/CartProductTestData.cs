using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for the CartProduct entity.
/// </summary>
public static class CartProductTestData
{
    public static CartProduct GenerateValidCartProduct()
        => new CartProduct(Guid.NewGuid(), 5);
}