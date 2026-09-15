namespace GreenBill.Merchant.Domain.Entity
{
    public class MerchantUser
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public string Role { get; set; } = null!;
    }
}
