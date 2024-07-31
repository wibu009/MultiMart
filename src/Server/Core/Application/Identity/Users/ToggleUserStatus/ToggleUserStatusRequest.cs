using System.Text.Json.Serialization;

namespace MultiMart.Application.Identity.Users.ToggleUserStatus;

public class ToggleUserStatusRequest : IRequest<Unit>
{
    public bool ActivateUser { get; set; }
    [JsonIgnore]
    public string? UserId { get; set; }
}
