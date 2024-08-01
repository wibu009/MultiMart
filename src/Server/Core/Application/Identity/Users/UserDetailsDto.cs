using MultiMart.Application.Common.Interfaces;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Application.Identity.Users;

public class UserDetailsDto : IDto
{
    public DefaultIdType Id { get; set; }
    public string? UserName { get; set; }
    public Gender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Avatar { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    public bool EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
}