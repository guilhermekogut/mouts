using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    /// <summary>
    /// Query for retrieving a cart by its Id.
    /// </summary>
    public class GetCartCommand : IRequest<GetCartResult>
    {
        public Guid Id { get; set; }

        public GetCartCommand()
        {
        }

        public GetCartCommand(Guid id)
        {
            Id = id;
        }
    }
}