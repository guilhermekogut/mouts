namespace Ambev.DeveloperEvaluation.Domain.Specifications;

public class SaleTotalConsistencySpecification : ISpecification<Sale>
{
    public bool IsSatisfiedBy(Sale sale)
    {
        var sum = sale.Items.Where(w => w.Cancelled == false).Sum(i => i.Total);
        return sale.TotalAmount == sum;
    }
}