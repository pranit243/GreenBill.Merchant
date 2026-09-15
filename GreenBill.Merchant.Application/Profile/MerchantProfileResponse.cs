namespace GreenBill.Merchant.Application.Profile
{
    public class MerchantProfileResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
