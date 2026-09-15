using GreenBill.Merchant.Domain.Entity;

namespace GreenBill.Merchant.Application.Common.Interfaces
{
    /// <summary>Application-layer contract; implemented in Infrastructure against MerchantDbContext.</summary>
    public interface IMerchantUserRepository
    {
        Task<MerchantUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<MerchantUser?> FindAsync(Guid id, CancellationToken cancellationToken = default);

        Task AddAsync(MerchantUser user, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
