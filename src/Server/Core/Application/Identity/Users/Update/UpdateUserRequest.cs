using System.Text.Json.Serialization;
using MultiMart.Application.Common.FileStorage;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Application.Identity.Users.Update;

public class UpdateUserRequest : IRequest<string>
{
    [JsonIgnore]
    public string Id { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public FileUpload? Image { get; set; }
    public bool DeleteCurrentImage { get; set; } = false;
}

public class UpdateCustomerRequest : UpdateUserRequest
{
    public int LoyaltyPoints { get; set; }
}

public class UpdateEmployeeRequest : UpdateUserRequest
{
    public string? Position { get; set; }
    public string? Department { get; set; }
    public DateTime? HireDate { get; set; }
    public string? ManagerId { get; set; }
}