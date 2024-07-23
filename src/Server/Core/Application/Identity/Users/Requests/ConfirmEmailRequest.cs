using MultiMart.Application.Identity.Users.Interfaces;

namespace MultiMart.Application.Identity.Users.Requests;

public class ConfirmEmailRequest : IRequest<string>
{
    public string Token { get; set; }
    public string UserId { get; set; }

    public ConfirmEmailRequest(string token, string userId)
    {
        Token = token;
        UserId = userId;
    }
}

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