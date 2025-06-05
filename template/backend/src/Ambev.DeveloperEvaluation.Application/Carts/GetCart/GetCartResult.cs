using Ambev.DeveloperEvaluation.Application.Carts.Common;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    /// <summary>
    /// Result returned after retrieving a cart by Id.
    /// </summary>
    public class GetCartResult
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CartProductItemResult> Products { get; set; } = new();
    }
}