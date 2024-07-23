using System.Text.Json.Serialization;
using MultiMart.Application.Identity.Users.Interfaces;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Application.Identity.Users.Requests;

public class CreateUserRequest : IRequest<string>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    [JsonIgnore]
    public string? Origin { get; set; }
}

public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, string>
{
    private readonly IUserService _userService;

    public CreateUserRequestHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        return await _userService.CreateAsync(request, cancellationToken);
    }
}