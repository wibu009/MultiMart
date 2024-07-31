namespace MultiMart.Application.Identity.Users.Create;

public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, string>
{
    private readonly IUserService _userService;

    public CreateUserRequestHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        return await _userService.CreateAsync(request, cancellationToken);
    }
}