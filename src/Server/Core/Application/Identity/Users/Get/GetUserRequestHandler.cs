namespace MultiMart.Application.Identity.Users.Get;

public class GetUserRequestHandler : IRequestHandler<GetUserRequest, UserDetailsDto>
{
    private readonly IUserService _userService;

    public GetUserRequestHandler(IUserService userService) => _userService = userService;

    public Task<UserDetailsDto> Handle(GetUserRequest request, CancellationToken cancellationToken)
        => _userService.GetAsync(request.Id, cancellationToken);
}