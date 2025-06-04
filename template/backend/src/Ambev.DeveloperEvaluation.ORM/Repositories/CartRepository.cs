using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<Cart> _carts;

        public CartRepository(DbContext context)
        {
            _context = context;
            _carts = context.Set<Cart>();
        }

        public async Task AddAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            await _carts.AddAsync(cart, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _carts
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _carts
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
        }

        public IQueryable<Cart> Query()
        {
            return _carts.Include(c => c.Products).AsQueryable();

        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var cart = await _carts.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (cart != null)
            {
                _carts.Remove(cart);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            _carts.Update(cart);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}