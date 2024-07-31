namespace MultiMart.Application.Identity.Users.Update;

public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest>
{
    private readonly IUserService _userService;

    public UpdateUserRequestHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Unit> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        await _userService.UpdateAsync(request, cancellationToken);

        return Unit.Value;
    }
}