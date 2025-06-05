namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSale
{
    /// <summary>
    /// Request model for creating a new sale based on a cart.
    /// </summary>
    public class CreateSaleRequest
    {
        /// <summary>
        /// The unique identifier of the cart to be converted into a sale.
        /// </summary>
        public Guid CartId { get; set; }
    }
}