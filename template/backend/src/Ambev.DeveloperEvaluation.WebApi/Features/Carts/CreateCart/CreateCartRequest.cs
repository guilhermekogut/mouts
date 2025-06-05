using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    /// <summary>
    /// Request model for creating a new cart.
    /// </summary>
    public class CreateCartRequest
    {
        /// <summary>
        /// The unique identifier of the user who owns the cart.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The date and time the cart is created.
        /// </summary>
        public DateTime Date { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The list of products to be added to the cart.
        /// </summary>
        public List<CartProductItemRequest> Products { get; set; } = new();
    }
}