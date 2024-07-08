using MultiMart.Application.Identity.Users.Interfaces;

namespace MultiMart.Application.Identity.Users.Requests.Commands;

public class ConfirmPhoneNumberRequest : IRequest<string>
{
    public string Token { get; set; }
    public string UserId { get; set; }

    public ConfirmPhoneNumberRequest(string token, string userId)
    {
        Token = token;
        UserId = userId;
    }
}

public class ConfirmPhoneNumberRequestHandler : IRequestHandler<ConfirmPhoneNumberRequest, string>
{
    private readonly IUserService _userService;

    public ConfirmPhoneNumberRequestHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(ConfirmPhoneNumberRequest request, CancellationToken cancellationToken)
    {
        return await _userService.ConfirmPhoneNumberAsync(request.UserId, request.Token);
    }
}