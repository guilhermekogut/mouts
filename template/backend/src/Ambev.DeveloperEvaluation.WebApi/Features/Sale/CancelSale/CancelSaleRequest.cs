namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.CancelSale
{
    /// <summary>
    /// Request model for cancelling an entire sale.
    /// </summary>
    public class CancelSaleRequest
    {
        /// <summary>
        /// The unique identifier of the sale to cancel.
        /// </summary>
        public Guid SaleId { get; set; }
    }
}