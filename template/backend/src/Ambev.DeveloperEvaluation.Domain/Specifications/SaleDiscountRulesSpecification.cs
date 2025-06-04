namespace Ambev.DeveloperEvaluation.Domain.Specifications;

public class SaleDiscountRulesSpecification : ISpecification<Sale>
{
    public bool IsSatisfiedBy(Sale sale)
    {
        foreach (var item in sale.Items)
        {
            if (item.Quantity < 4 && item.Discount > 0)
                return false;
            if (item.Quantity >= 4 && item.Quantity < 10 && item.Discount > item.UnitPrice * item.Quantity * 0.10m)
                return false;
            if (item.Quantity >= 10 && item.Quantity <= 20 && item.Discount > item.UnitPrice * item.Quantity * 0.20m)
                return false;
            if (item.Quantity > 20)
                return false;
        }
        return true;
    }
}