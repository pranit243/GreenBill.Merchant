using GreenBill.Merchant.Application.Common.Interfaces;
using MediatR;

namespace GreenBill.Merchant.Application.Profile
{
    public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, MerchantProfileResponse>
    {
        private readonly IMerchantUserRepository _merchantUserRepository;

        public GetMyProfileQueryHandler(IMerchantUserRepository merchantUserRepository)
        {
            _merchantUserRepository = merchantUserRepository;
        }

        public async Task<MerchantProfileResponse> Handle(
            GetMyProfileQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _merchantUserRepository.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new KeyNotFoundException("Merchant profile not found.");

            return new MerchantProfileResponse
            {
                UserId = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive = user.IsActive
            };
        }
    }
}
