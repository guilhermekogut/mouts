using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Specifications;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a shopping cart in the system.
/// </summary>
public class Cart : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProduct> Products { get; set; } = new();

    public Cart()
    {
        Date = DateTime.UtcNow;
    }

    public Cart(Guid userId, DateTime date, IEnumerable<CartProduct> products)
    {
        UserId = userId;
        Date = date;
        Products = products?.ToList() ?? new List<CartProduct>();
    }

    /// <summary>
    /// Adds a product to the cart or updates its quantity if it already exists.
    /// Applies business rules for quantity limits.
    /// </summary>
    public void AddOrUpdateProduct(Guid productId, int quantity)
    {
        if (quantity < 1)
            throw new InvalidOperationException("The quantity must be greater than zero.");

        if (quantity > 20)
            throw new InvalidOperationException("It is not allowed to sell more than 20 items of the same product.");

        var existing = Products.FirstOrDefault(p => p.ProductId == productId);
        if (existing != null)
        {
            existing.Quantity = quantity;
        }
        else
        {
            Products.Add(new CartProduct(productId, quantity));
        }
        Date = DateTime.UtcNow;
    }

    /// <summary>
    /// Remove a product from the cart.
    /// </summary>
    public void RemoveProduct(Guid productId)
    {
        var existing = Products.FirstOrDefault(p => p.ProductId == productId);
        if (existing != null)
        {
            Products.Remove(existing);
            Date = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Clears all products from the cart.
    /// </summary>
    public void Clear()
    {
        Products.Clear();
        Date = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the discount percentage for a given quantity of a product.
    /// </summary>
    public static decimal GetDiscountPercentage(int quantity)
    {
        if (quantity >= 10 && quantity <= 20)
            return 0.20m;
        if (quantity >= 4 && quantity < 10)
            return 0.10m;
        return 0m;
    }

    /// <summary>
    /// Calculates the total for a specific product in the cart, applying discount rules.
    /// </summary>
    /// <param name="productId">The product ID.</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    /// <returns>Total value for the item, with discount applied if applicable.</returns>
    public decimal GetItemTotal(Guid productId, decimal unitPrice)
    {
        var item = Products.FirstOrDefault(p => p.ProductId == productId);
        if (item == null)
            return 0m;

        var discount = GetDiscountPercentage(item.Quantity);
        return item.Quantity * unitPrice * (1 - discount);
    }

    /// <summary>
    /// Calculates the total value of the cart, applying discounts per item.
    /// </summary>
    /// <param name="getUnitPrice">A function to retrieve the unit price for a given productId.</param>
    /// <returns>Total value of the cart.</returns>
    public decimal GetCartTotal(Func<Guid, decimal> getUnitPrice)
    {
        if (getUnitPrice == null)
            throw new ArgumentNullException(nameof(getUnitPrice));

        decimal total = 0m;
        foreach (var item in Products)
        {
            var unitPrice = getUnitPrice(item.ProductId);
            var discount = GetDiscountPercentage(item.Quantity);
            total += item.Quantity * unitPrice * (1 - discount);
        }
        return total;
    }

    /// <summary>
    /// Validates all business rules for the cart and its items.
    /// Throws exception if any rule is violated.
    /// </summary>
    public void ValidateBusinessRules()
    {
        var quantitySpec = new CartProductQuantitySpecification();
        if (!quantitySpec.IsSatisfiedBy(this))
            throw new InvalidOperationException("Each product quantity must be between 1 and 20.");

        var duplicateSpec = new CartNoDuplicateProductsSpecification();
        if (!duplicateSpec.IsSatisfiedBy(this))
            throw new InvalidOperationException("Duplicate products are not allowed in the cart.");
    }
}