namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for the SaleItem entity.
/// </summary>
public static class SaleItemTestData
{
    public static SaleItem GenerateValidSaleItem()
        => new SaleItem
        {
            Id = Guid.NewGuid(),
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            ProductName = "Test Product",
            Quantity = 5,
            UnitPrice = 100m,
            Discount = 10m,
            Total = 490m,
            Cancelled = false
        };

    public static SaleItem GenerateCancelledSaleItem()
        => new SaleItem
        {
            Id = Guid.NewGuid(),
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            ProductName = "Cancelled Product",
            Quantity = 2,
            UnitPrice = 50m,
            Discount = 0m,
            Total = 100m,
            Cancelled = true
        };
}