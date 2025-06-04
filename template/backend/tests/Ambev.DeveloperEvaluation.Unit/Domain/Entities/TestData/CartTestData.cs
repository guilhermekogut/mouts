using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for the Cart entity.
/// </summary>
public static class CartTestData
{
    public static Cart GenerateEmptyCart()
        => new Cart();

    public static Cart GenerateCartWithOneProduct()
    {
        var product = CartProductTestData.GenerateValidCartProduct();
        return new Cart(Guid.NewGuid(), DateTime.UtcNow, new[] { product });
    }

    public static Cart GenerateCartWithDiscountProduct()
    {
        var product = new CartProduct(Guid.NewGuid(), 10); // triggers 20% discount
        return new Cart(Guid.NewGuid(), DateTime.UtcNow, new[] { product });
    }

    public static Cart GenerateCartWithInvalidProduct()
    {
        var product = new CartProduct(Guid.NewGuid(), 0); // invalid quantity
        return new Cart(Guid.NewGuid(), DateTime.UtcNow, new[] { product });
    }
}