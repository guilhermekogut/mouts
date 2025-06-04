using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.Common;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

public static class CancelSaleHandlerTestData
{
    private static readonly Faker<SaleItemResult> ItemFaker = new Faker<SaleItemResult>("pt_BR")
        .RuleFor(i => i.Id, f => f.Random.Guid())
        .RuleFor(i => i.ProductId, f => f.Random.Guid())
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 5))
        .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(10, 100))
        .RuleFor(i => i.Discount, f => f.Random.Decimal(0, 10))
        .RuleFor(i => i.Total, (f, i) => i.Quantity * i.UnitPrice - i.Discount)
        .RuleFor(i => i.Cancelled, true);

    private static readonly Faker<CancelSaleCommand> CommandFaker = new Faker<CancelSaleCommand>("pt_BR")
        .RuleFor(c => c.SaleId, f => f.Random.Guid())
        .RuleFor(c => c.AuthenticatedUserId, f => f.Random.Guid());

    public static CancelSaleCommand GenerateValidCommand() => CommandFaker.Generate();

    public static CancelSaleCommand GenerateCommandWithInvalidSaleId()
    {
        var command = CommandFaker.Generate();
        command.SaleId = Guid.Empty;
        return command;
    }

    public static Sale GenerateSaleFromCommand(CancelSaleCommand command, bool alreadyCancelled = false)
    {
        var items = new List<SaleItem>
        {
            new SaleItem
            {
                Id = Guid.NewGuid(),
                SaleId = command.SaleId,
                ProductId = Guid.NewGuid(),
                ProductName = "Test Product",
                Quantity = 2,
                UnitPrice = 50m,
                Discount = 0m,
                Total = 100m,
                Cancelled = alreadyCancelled
            }
        };
        return new Sale
        {
            Id = command.SaleId,
            CustomerId = command.AuthenticatedUserId,
            CustomerName = "Test User",
            Date = DateTime.UtcNow,
            Items = items,
            TotalAmount = alreadyCancelled ? 0m : 100m,
            Cancelled = alreadyCancelled
        };
    }

    public static CancelSaleResult GenerateResultFromSale(Sale sale)
    {
        return new CancelSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            Date = sale.Date,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            TotalAmount = sale.TotalAmount,
            Cancelled = sale.Cancelled,
            Items = sale.Items.Select(i => new SaleItemResult
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                Total = i.Total,
                Cancelled = i.Cancelled
            }).ToList()
        };
    }
}