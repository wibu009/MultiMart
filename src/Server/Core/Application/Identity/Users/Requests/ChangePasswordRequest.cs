using MultiMart.Application.Common.Interfaces;
using MultiMart.Application.Identity.Users.Interfaces;

namespace MultiMart.Application.Identity.Users.Requests;

public class ChangePasswordRequest : IRequest
{
    public string Password { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmNewPassword { get; set; } = default!;
}

public class ChangePasswordRequestHandler : IRequestHandler<ChangePasswordRequest>
{
    private readonly IUserService _userService;
    private readonly ICurrentUser _currentUser;

    public ChangePasswordRequestHandler(IUserService userService, ICurrentUser currentUser)
    {
        _userService = userService;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        await _userService
            .ChangePasswordAsync(
                request.Password,
                request.NewPassword,
                _currentUser.GetUserId().ToString());
        return Unit.Value;
    }
}