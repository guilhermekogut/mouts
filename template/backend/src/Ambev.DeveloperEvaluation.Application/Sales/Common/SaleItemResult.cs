namespace Ambev.DeveloperEvaluation.Application.Sales.Common
{
    public class SaleItemResult
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public bool Cancelled { get; set; } = false;
    }
}
