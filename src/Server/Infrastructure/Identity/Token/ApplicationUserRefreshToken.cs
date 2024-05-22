using BookStack.Infrastructure.Identity.User;

namespace BookStack.Infrastructure.Identity.Token;

public class ApplicationUserRefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = default!;
    public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(30);
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime? Revoked { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;
    public string UserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = default!;
}