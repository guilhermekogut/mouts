namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSale
{
    /// <summary>
    /// Request model for updating (cancelling an item from) a sale.
    /// </summary>
    public class UpdateSaleRequest
    {
        /// <summary>
        /// The unique identifier of the sale to update.
        /// </summary>
        public Guid SaleId { get; set; }

        /// <summary>
        /// The unique identifier of the product to cancel in the sale.
        /// </summary>
        public Guid ProductId { get; set; }
    }
}