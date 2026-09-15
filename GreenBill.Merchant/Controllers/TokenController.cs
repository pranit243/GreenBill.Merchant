using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GreenBill.Merchant.API.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        [Authorize]
        [HttpGet("check")]
        public IActionResult Check()
        {
            var expiresAt = User.FindFirstValue(JwtRegisteredClaimNames.Exp)
                ?? User.FindFirstValue("exp");

            return Ok(new
            {
                Valid = true,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub),
                Email = User.FindFirstValue(ClaimTypes.Email)
                    ?? User.FindFirstValue(JwtRegisteredClaimNames.Email),
                Role = User.FindFirstValue(ClaimTypes.Role),
                Issuer = User.FindFirstValue(JwtRegisteredClaimNames.Iss),
                Audience = User.FindFirstValue(JwtRegisteredClaimNames.Aud),
                ExpiresAtUtc = expiresAt is null
                    ? (DateTime?)null
                    : DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiresAt)).UtcDateTime
            });
        }
    }
}