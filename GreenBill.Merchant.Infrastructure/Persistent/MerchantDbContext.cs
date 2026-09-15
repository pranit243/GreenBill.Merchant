using GreenBill.Merchant.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace GreenBill.Merchant.Infrastructure.Persistent
{
    public class MerchantDbContext:DbContext
    {
        public MerchantDbContext(DbContextOptions<MerchantDbContext> option):base(option)
        {

        }

        public DbSet<MerchantUser> MerchantUsers { get; set; }
    }
}
