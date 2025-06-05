namespace Ambev.DeveloperEvaluation.Domain.Specifications;

public class SaleNoNegativeValuesSpecification : ISpecification<Sale>
{
    public bool IsSatisfiedBy(Sale sale)
    {
        if (sale.TotalAmount < 0)
            return false;
        foreach (var item in sale.Items)
        {
            if (item.UnitPrice < 0 || item.Discount < 0 || item.Total < 0)
                return false;
        }
        return true;
    }
}