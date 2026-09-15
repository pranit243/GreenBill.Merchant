using System.Net.Http.Json;
using GreenBill.Merchant.Application.Common.Http;

namespace GreenBill.Merchant.Infrastructure.Http
{
    /// <summary>
    /// Sample synchronous HTTP client to Identity, registered as a typed client
    /// (see doc.md section 6). Not on the hot path today - use sparingly, prefer
    /// reading claims already present in the caller's JWT where possible.
    /// </summary>
    public class IdentityServiceClient : IIdentityServiceClient
    {
        private readonly HttpClient _httpClient;

        public IdentityServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IdentityUserInfo?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync($"api/users/{userId}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<IdentityUserInfo>(cancellationToken);
        }
    }
}
