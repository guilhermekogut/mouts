namespace Ambev.DeveloperEvaluation.Domain.Specifications;

public class SaleProductQuantitySpecification : ISpecification<Sale>
{
    public bool IsSatisfiedBy(Sale sale)
    {
        return sale.Items.All(i => i.Quantity >= 1 && i.Quantity <= 20);
    }
}