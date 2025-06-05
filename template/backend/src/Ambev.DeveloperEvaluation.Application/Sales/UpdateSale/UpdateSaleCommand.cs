using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Command to update (cancel an item from) a sale.
    /// </summary>
    public class UpdateSaleCommand : IRequest<UpdateSaleResult>
    {
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
        public Guid AuthenticatedUserId { get; set; }
    }
}