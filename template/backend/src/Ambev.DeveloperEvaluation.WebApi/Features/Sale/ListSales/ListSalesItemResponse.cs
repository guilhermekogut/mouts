using Ambev.DeveloperEvaluation.WebApi.Features.Sale.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.ListSales
{
    public class ListSalesItemResponse
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