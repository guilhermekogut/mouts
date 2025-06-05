using Ambev.DeveloperEvaluation.Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<Sale> _sales;

        public SaleRepository(DbContext context)
        {
            _context = context;
            _sales = context.Set<Sale>();
        }

        public async Task AddAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            await _sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = await _sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
            if (sale != null)
            {
                _sales.Remove(sale);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public IQueryable<Sale> Query()
        {
            return _sales.Include(s => s.Items).AsQueryable();
        }

        public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            _sales.Update(sale);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}