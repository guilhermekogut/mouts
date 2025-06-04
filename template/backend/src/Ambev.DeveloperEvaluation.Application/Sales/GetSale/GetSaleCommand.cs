using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Query for retrieving a sale by its Id.
    /// </summary>
    public class GetSaleCommand : IRequest<GetSaleResult>
    {
        public Guid Id { get; set; }

        public GetSaleCommand() { }

        public GetSaleCommand(Guid id)
        {
            Id = id;
        }
    }
}