namespace Ambev.DeveloperEvaluation.Domain.Specifications;

public class SaleDateNotInFutureSpecification : ISpecification<Sale>
{
    public bool IsSatisfiedBy(Sale sale)
    {
        return sale.Date <= DateTime.UtcNow;
    }
}