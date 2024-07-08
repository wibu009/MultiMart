using System.Text.Json.Serialization;
using MultiMart.Application.Identity.Users.Interfaces;

namespace MultiMart.Application.Identity.Users.Requests.Commands;

public class ForgotPasswordRequest : IRequest<string>
{
    public string Email { get; set; } = default!;
    [JsonIgnore]
    public string? Origin { get; set; } = default!;
}

public class ForgotPasswordRequestHandler : IRequestHandler<ForgotPasswordRequest, string>
{
    private readonly IUserService _userService;

    public ForgotPasswordRequestHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        return await _userService.ForgotPasswordAsync(request.Email, request.Origin, cancellationToken);
    }
}