namespace MultiMart.Application.Identity.Users.ForgotPassword;

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