using Ambev.DeveloperEvaluation.WebApi.Features.Sale.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSale
{
    /// <summary>
    /// Response model returned after updating a sale.
    /// </summary>
    public class UpdateSaleResponse
    {
        public Guid Id { get; set; }
        public int SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public bool Cancelled { get; set; }
        public List<SaleItemResponse> Items { get; set; } = new();
    }
}