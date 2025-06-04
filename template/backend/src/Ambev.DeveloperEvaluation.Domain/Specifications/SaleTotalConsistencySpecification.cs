namespace Ambev.DeveloperEvaluation.Domain.Specifications;

public class SaleTotalConsistencySpecification : ISpecification<Sale>
{
    public bool IsSatisfiedBy(Sale sale)
    {
        var sum = sale.Items.Sum(i => i.Total);
        return sale.TotalAmount == sum;
    }
}