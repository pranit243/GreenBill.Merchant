namespace GreenBill.Merchant.Application.Common.Http
{
    /// <summary>
    /// Typed HTTP client contract for the rare synchronous, immediate-response calls to the
    /// Identity service (see doc.md section 6, "Communication Model").
    /// Implemented in the Infrastructure layer with a named/typed HttpClient.
    /// </summary>
    public interface IIdentityServiceClient
    {
        Task<IdentityUserInfo?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }

    public record IdentityUserInfo(Guid UserId, string Email, string Role, bool IsActive);
}
