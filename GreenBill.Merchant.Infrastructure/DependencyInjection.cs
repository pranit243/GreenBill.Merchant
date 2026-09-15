using GreenBill.Merchant.Application.Common.Http;
using GreenBill.Merchant.Application.Common.Interfaces;
using GreenBill.Merchant.Infrastructure.Http;
using GreenBill.Merchant.Infrastructure.Persistent;
using GreenBill.Merchant.Infrastructure.Persistent.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GreenBill.Merchant.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<MerchantDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MerchantDatabase")));

            services.AddScoped<IMerchantUserRepository, MerchantUserRepository>();

            services.AddHttpClient<IIdentityServiceClient, IdentityServiceClient>(client =>
            {
                var baseUrl = configuration["Services:IdentityBaseUrl"]
                    ?? throw new InvalidOperationException("Services:IdentityBaseUrl is not configured.");

                client.BaseAddress = new Uri(baseUrl);
            });

            return services;
        }
    }
}
