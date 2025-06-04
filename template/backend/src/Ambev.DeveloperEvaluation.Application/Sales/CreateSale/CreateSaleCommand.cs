using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Command to create a new sale based on a cart.
    /// </summary>
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        /// <summary>
        /// The unique identifier of the cart to be converted into a sale.
        /// </summary>
        public Guid CartId { get; set; }
        public Guid AuthenticatedUserId { get; set; }
    }
}