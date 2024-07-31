namespace MultiMart.Application.Identity.Users.ConfirmEmail;

public class ConfirmEmailRequestHandler : IRequestHandler<ConfirmEmailRequest, string>
{
    private readonly IUserService _userService;

    public ConfirmEmailRequestHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        return await _userService.ConfirmEmailAsync(request.UserId, request.Token, cancellationToken);
    }
}