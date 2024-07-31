namespace MultiMart.Application.Identity.Users.ConfirmPhoneNumber;

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