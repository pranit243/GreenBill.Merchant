using GreenBill.Merchant.Domain.Entity;
using GreenBill.Merchant.Infrastructure.Persistent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace GreenBill.Merchant.API.Filter
{
    public class EnsureUserExistsActionFilter : IAsyncActionFilter
    {
        private readonly MerchantDbContext _dbContext;

        public EnsureUserExistsActionFilter(MerchantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userPrincipal = context.HttpContext.User;

            if (userPrincipal.Identity?.IsAuthenticated == true)
            {
                var userIdStr = userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? userPrincipal.FindFirst("sub")?.Value;

                if (!string.IsNullOrEmpty(userIdStr))
                {
                    if (!Guid.TryParse(userIdStr, out var userGuid))
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                    var email = userPrincipal.FindFirst(ClaimTypes.Email)?.Value
                                ?? userPrincipal.FindFirst("email")?.Value;

                    var mobile = userPrincipal.FindFirst(ClaimTypes.MobilePhone)?.Value;

                    var localUser = await _dbContext.MerchantUsers.FindAsync(userGuid);

                    if (localUser == null)
                    {
                        var newProfile = new MerchantUser
                        {
                            Id = userGuid,
                            Email = email ?? string.Empty,
                            PhoneNumber = mobile ?? string.Empty,
                            IsActive = true,
                            CreatedAtUtc = DateTime.UtcNow,
                            Role = userPrincipal.FindFirst(ClaimTypes.Role)?.Value ?? "Merchant"
                        };

                        await _dbContext.MerchantUsers.AddAsync(newProfile);
                        await _dbContext.SaveChangesAsync(context.HttpContext.RequestAborted);
                    }
                    else
                    {
                        if (localUser.Email != email || localUser.PhoneNumber != mobile)
                        {
                            localUser.Email = email ?? string.Empty;
                            localUser.PhoneNumber = mobile ?? string.Empty;

                            _dbContext.MerchantUsers.Update(localUser);
                            await _dbContext.SaveChangesAsync(context.HttpContext.RequestAborted);
                        }
                    }
                }
            }
            await next();
        }
    }
}
