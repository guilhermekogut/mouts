using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

public class CartNoDuplicateProductsSpecification : ISpecification<Cart>
{
    public bool IsSatisfiedBy(Cart cart)
    {
        return cart.Products.Select(p => p.ProductId).Distinct().Count() == cart.Products.Count;
    }
}