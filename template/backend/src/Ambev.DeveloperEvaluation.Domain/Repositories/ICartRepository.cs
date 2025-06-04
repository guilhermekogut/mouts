using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Repository interface for managing shopping carts.
    /// </summary>
    public interface ICartRepository
    {
        /// <summary>
        /// Retrieves a cart by its unique identifier.
        /// </summary>
        Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a cart by the associated user ID.
        /// </summary>
        Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves queryable collection of carts.
        /// </summary>
        IQueryable<Cart> Query();

        /// <summary>
        /// Adds a new cart.
        /// </summary>
        Task AddAsync(Cart cart, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing cart.
        /// </summary>
        Task UpdateAsync(Cart cart, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delele a cart by its unique identifier.
        /// </summary>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);


    }
}