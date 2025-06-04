using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart
{
    /// <summary>
    /// Request model for updating an existing cart.
    /// </summary>
    public class UpdateCartRequest
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CartProductItemRequest> Products { get; set; } = new();
    }
}