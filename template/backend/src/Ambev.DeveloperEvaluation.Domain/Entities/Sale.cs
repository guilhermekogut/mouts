using Ambev.DeveloperEvaluation.Domain.Specifications;

public class Sale
{
    public Guid Id { get; set; }
    public int SaleNumber { get; set; }
    public DateTime Date { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public bool Cancelled { get; set; }
    public List<SaleItem> Items { get; set; } = [];


    public static decimal GetDiscountAmount(decimal unitPrice, int quantity)
    {
        var percentage = GetDiscountPercentage(quantity);
        if (percentage == 0) return 0;
        return unitPrice * quantity * percentage;
    }
    public static decimal GetDiscountPercentage(int quantity)
    {
        if (quantity >= 10 && quantity <= 20)
            return 0.20m;
        if (quantity >= 4)
            return 0.10m;
        return 0m;
    }

    public void Validate()
    {
        var specifications = new ISpecification<Sale>[]
        {
            new SaleAtLeastOneItemSpecification(),
            new SaleNoDuplicateProductsSpecification(),
            new SaleProductQuantitySpecification(),
            new SaleDiscountRulesSpecification(),
            new SaleTotalConsistencySpecification(),
            new SaleNoNegativeValuesSpecification(),
            new SaleDateNotInFutureSpecification(),
            new SaleCancelledItemsSpecification()
        };

        var failed = specifications.Where(s => !s.IsSatisfiedBy(this)).ToList();
        if (failed.Any())
        {
            var names = string.Join(", ", failed.Select(f => f.GetType().Name));
            throw new InvalidOperationException($"Sale validation failed: {names}");
        }
    }
}