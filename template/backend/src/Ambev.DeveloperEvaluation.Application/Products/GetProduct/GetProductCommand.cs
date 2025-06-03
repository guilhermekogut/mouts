using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    /// <summary>
    /// Query for retrieving a product by its Id.
    /// </summary>
    public class GetProductCommand : IRequest<GetProductResult>
    {
        public Guid Id { get; set; }
        public GetProductCommand()
        {

        }

        public GetProductCommand(Guid id)
        {
            Id = id;
        }
    }
}
