using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class CartProduct : BaseEntity
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public Product? Product { get; set; }

        public CartProduct() { }

        public CartProduct(Guid productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
