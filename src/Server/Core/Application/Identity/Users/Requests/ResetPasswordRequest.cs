using MultiMart.Application.Identity.Users.Interfaces;

namespace MultiMart.Application.Identity.Users.Requests;

public class ResetPasswordRequest : IRequest<string>
{
    public required string UserId { get; set; }

    public string? Password { get; set; }

    public string? ConfirmPassword { get; set; }

    public string? Token { get; set; }
}

public class ResetPasswordRequestHandler : IRequestHandler<ResetPasswordRequest, string>
{
    private readonly IUserService _userService;

    public ResetPasswordRequestHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        return await _userService.ResetPasswordAsync(request.UserId, request.Password, request.Token);
    }
}