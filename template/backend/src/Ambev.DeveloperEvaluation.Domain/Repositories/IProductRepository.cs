﻿using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Results;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Repository interface for Product entity operations
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Creates a new product in the repository
        /// </summary>
        /// <param name="product">The product to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created product</returns>
        Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing product in the repository
        /// </summary>
        /// <param name="product">The product to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated product</returns>
        Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a product by its unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the product</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The product if found, null otherwise</returns>
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a product from the repository
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the product was deleted, false if not found</returns>
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists all products in the repository for query purposes
        /// </summary>
        IQueryable<Product> Query();

        /// <summary>
        /// Retrieves all product categories
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of category names</returns>
        Task<IEnumerable<string>> GetCategoriesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves products in a specific category
        /// </summary>
        /// <param name="category">Category name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Queryable of products in the category</returns>
        IQueryable<Product> QueryByCategory(string category);

        /// <summary>
        /// Checks existence of multiple products by their IDs.
        /// </summary>
        /// <param name="productIds">Array of product IDs to check.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Strong type result with existence info for each product and overall status.</returns>
        Task<ProductExistenceResult> CheckExistenceAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken = default);
    }
}
