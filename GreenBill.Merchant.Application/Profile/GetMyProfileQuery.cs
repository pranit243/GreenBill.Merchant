using MediatR;

namespace GreenBill.Merchant.Application.Profile
{
    /// <summary>CQRS query: reads the merchant profile of the currently authenticated user.</summary>
    public record GetMyProfileQuery(Guid UserId) : IRequest<MerchantProfileResponse>;
}
