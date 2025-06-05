namespace Ambev.DeveloperEvaluation.Domain.Specifications;

public class SaleCancelledItemsSpecification : ISpecification<Sale>
{
    public bool IsSatisfiedBy(Sale sale)
    {
        if (!sale.Cancelled)
            return true;
        return sale.Items.All(i => i.Cancelled);
    }
}