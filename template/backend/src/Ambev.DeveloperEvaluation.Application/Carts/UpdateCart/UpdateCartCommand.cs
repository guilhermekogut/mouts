using Ambev.DeveloperEvaluation.Application.Carts.Common;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    /// <summary>
    /// Command to update an existing shopping cart.
    /// </summary>
    public class UpdateCartCommand : IRequest<UpdateCartResult>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CartProductItemResult> Products { get; set; } = [];
    }
}