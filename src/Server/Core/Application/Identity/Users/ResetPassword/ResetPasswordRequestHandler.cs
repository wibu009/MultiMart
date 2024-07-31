namespace MultiMart.Application.Identity.Users.ResetPassword;

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