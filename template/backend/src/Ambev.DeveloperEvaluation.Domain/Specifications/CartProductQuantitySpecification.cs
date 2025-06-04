using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

public class CartProductQuantitySpecification : ISpecification<Cart>
{
    public bool IsSatisfiedBy(Cart cart)
    {
        return cart.Products.All(p => p.Quantity >= 1 && p.Quantity <= 20);
    }
}