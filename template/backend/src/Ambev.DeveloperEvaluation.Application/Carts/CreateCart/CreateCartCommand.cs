using Ambev.DeveloperEvaluation.Application.Carts.Common;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    /// <summary>
    /// Command to create a new shopping cart.
    /// </summary>
    public class CreateCartCommand : IRequest<CreateCartResult>
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
        public List<CartProductItemResult> Products { get; set; } = new();
    }
}
