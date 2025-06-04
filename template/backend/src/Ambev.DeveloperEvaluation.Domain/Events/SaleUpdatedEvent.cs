using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    /// <summary>
    /// Event raised when a sale is created.
    /// </summary>
    public class SaleUpdatedEvent : INotification
    {
        public SaleUpdatedEvent(Sale sale)
        {
            Sale = sale;
        }

        public Sale Sale { get; }
    }
}