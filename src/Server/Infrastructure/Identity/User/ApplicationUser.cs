using BookStack.Infrastructure.Identity.Token;
using Microsoft.AspNetCore.Identity;

namespace BookStack.Infrastructure.Identity.User;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string? ObjectId { get; set; }
    public ICollection<ApplicationUserRefreshToken> RefreshTokens { get; set; } = new List<ApplicationUserRefreshToken>();
}