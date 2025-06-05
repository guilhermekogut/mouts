using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Command to cancel an entire sale.
    /// </summary>
    public class CancelSaleCommand : IRequest<CancelSaleResult>
    {
        public Guid SaleId { get; set; }
        public Guid AuthenticatedUserId { get; set; }
    }
}