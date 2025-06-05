namespace Ambev.DeveloperEvaluation.Domain.Specifications;

public class SaleAtLeastOneItemSpecification : ISpecification<Sale>
{
    public bool IsSatisfiedBy(Sale sale)
    {
        return sale.Items != null && sale.Items.Count > 0;
    }
}