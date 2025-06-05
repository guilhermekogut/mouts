namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.ListSales
{
    public class ListSalesResponse
    {
        public IEnumerable<ListSalesItemResponse> Data { get; set; } = [];
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}