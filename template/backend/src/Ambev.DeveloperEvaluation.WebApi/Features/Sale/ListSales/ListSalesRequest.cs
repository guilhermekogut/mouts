namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.ListSales
{
    public class ListSalesRequest
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? Order { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public bool? Cancelled { get; set; }
    }
}