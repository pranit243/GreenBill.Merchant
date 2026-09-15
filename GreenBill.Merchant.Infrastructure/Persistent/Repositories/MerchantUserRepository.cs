using GreenBill.Merchant.Application.Common.Interfaces;
using GreenBill.Merchant.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace GreenBill.Merchant.Infrastructure.Persistent.Repositories
{
    public class MerchantUserRepository : IMerchantUserRepository
    {
        private readonly MerchantDbContext _context;

        public MerchantUserRepository(MerchantDbContext context)
        {
            _context = context;
        }

        public async Task<MerchantUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.MerchantUsers
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<MerchantUser?> FindAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.MerchantUsers.FindAsync(new object?[] { id }, cancellationToken);
        }

        public async Task AddAsync(MerchantUser user, CancellationToken cancellationToken = default)
        {
            await _context.MerchantUsers.AddAsync(user, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
