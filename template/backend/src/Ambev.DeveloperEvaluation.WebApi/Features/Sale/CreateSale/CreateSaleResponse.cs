using Ambev.DeveloperEvaluation.WebApi.Features.Sale.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSale
{
    /// <summary>
    /// Response model returned after creating a new sale.
    /// </summary>
    public class CreateSaleResponse
    {
        public Guid Id { get; set; }
        public int SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<SaleItemResponse> Items { get; set; } = new();
    }
}
