using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Repositories.Results;

using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    /// <summary>
    /// Implementation of IProductRepository using Entity Framework Core
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of ProductRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public ProductRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _context.Set<Product>().AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }

        /// <inheritdoc />
        public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            _context.Set<Product>().Update(product);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }

        /// <inheritdoc />
        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Product>()
                .Include(p => p.Rating)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await GetByIdAsync(id, cancellationToken);
            if (product == null)
                return false;

            _context.Set<Product>().Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <inheritdoc />
        public IQueryable<Product> Query()
        {
            return _context.Set<Product>()
                .Include(p => p.Rating)
                .AsQueryable();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<string>> GetCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Product>()
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public IQueryable<Product> QueryByCategory(string category)
        {
            return _context.Set<Product>()
                .Include(p => p.Rating)
                .Where(p => p.Category.ToLower() == category.ToLower());
        }

        /// <inheritdoc />
        public async Task<ProductExistenceResult> CheckExistenceAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken = default)
        {
            var ids = productIds.Distinct().ToList();

            var existingIds = await _context.Set<Product>()
                .Where(p => ids.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync(cancellationToken);

            var items = ids
                .Select(id => new ProductExistenceItem(id, existingIds.Contains(id)))
                .ToList();

            return new ProductExistenceResult(items);
        }
    }
}
