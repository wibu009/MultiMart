using Microsoft.AspNetCore.Identity;
using MultiMart.Infrastructure.Identity.Token;

namespace MultiMart.Infrastructure.Identity.User;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public string? ObjectId { get; set; }
    public ICollection<ApplicationUserRefreshToken> RefreshTokens { get; set; } = new List<ApplicationUserRefreshToken>();
}