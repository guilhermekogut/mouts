using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    /// <summary>
    /// Query for listing sales with pagination, ordering, and filtering.
    /// </summary>
    public class ListSalesCommand : IRequest<ListSalesResult>
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? Order { get; set; }

        // Filtering attributes
        public Guid? CustomerId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public bool? Cancelled { get; set; }
    }
}