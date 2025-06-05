namespace Ambev.DeveloperEvaluation.Application.Carts.Common
{
    /// <summary>
    /// Representing a product to be added to the cart.
    /// </summary>
    public class CartProductItemResult
    {
        /// <summary>
        /// The unique identifier of the product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The quantity of the product to add.
        /// </summary>
        public int Quantity { get; set; }
    }
}
