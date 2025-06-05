namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for the Sale entity.
/// </summary>
public static class SaleTestData
{
    public static Sale GenerateEmptySale()
        => new Sale();

    public static Sale GenerateSaleWithOneItem()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();
        return new Sale
        {
            Id = Guid.NewGuid(),
            Date = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            CustomerName = "Test Customer",
            Items = new List<SaleItem> { item },
            TotalAmount = item.Total
        };
    }

    public static Sale GenerateSaleWithMultipleItems()
    {
        var item1 = SaleItemTestData.GenerateValidSaleItem();
        var item2 = SaleItemTestData.GenerateValidSaleItem();
        item2.ProductId = Guid.NewGuid();
        item2.Total = 200m;
        return new Sale
        {
            Id = Guid.NewGuid(),
            Date = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            CustomerName = "Test Customer",
            Items = new List<SaleItem> { item1, item2 },
            TotalAmount = item1.Total + item2.Total
        };
    }

    public static Sale GenerateSaleWithInvalidItem()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();
        item.Quantity = 0; // Invalid quantity
        return new Sale
        {
            Id = Guid.NewGuid(),
            Date = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            CustomerName = "Test Customer",
            Items = new List<SaleItem> { item },
            TotalAmount = 0m
        };
    }

    public static Sale GenerateSaleWithAllItemsCancelled()
    {
        var item1 = SaleItemTestData.GenerateCancelledSaleItem();
        var item2 = SaleItemTestData.GenerateCancelledSaleItem();
        item2.ProductId = Guid.NewGuid();
        return new Sale
        {
            Id = Guid.NewGuid(),
            Date = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            CustomerName = "Test Customer",
            Items = new List<SaleItem> { item1, item2 },
            TotalAmount = 0m,
            Cancelled = true
        };
    }
}