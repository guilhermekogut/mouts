using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    /// <summary>
    /// Response model returned after creating a new cart.
    /// </summary>
    public class CreateCartResponse
    {
        /// <summary>
        /// The unique identifier of the created cart.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The unique identifier of the user who owns the cart.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The date and time the cart was created.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The list of products in the cart.
        /// </summary>
        public List<CartProductItemResponse> Products { get; set; } = new();
    }
}