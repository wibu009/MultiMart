using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Identity.Users.ChangePassword;

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