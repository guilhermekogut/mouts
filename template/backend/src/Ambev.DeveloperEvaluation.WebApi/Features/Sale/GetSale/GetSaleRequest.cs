namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetSale
{
    /// <summary>
    /// Request model for retrieving a sale by its Id.
    /// </summary>
    public class GetSaleRequest
    {
        public Guid Id { get; set; }
    }
}