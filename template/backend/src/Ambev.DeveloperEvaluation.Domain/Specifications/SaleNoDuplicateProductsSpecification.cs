namespace Ambev.DeveloperEvaluation.Domain.Specifications;

public class SaleNoDuplicateProductsSpecification : ISpecification<Sale>
{
    public bool IsSatisfiedBy(Sale sale)
    {
        return sale.Items.Select(i => i.ProductId).Distinct().Count() == sale.Items.Count;
    }
}