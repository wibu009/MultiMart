namespace MultiMart.Application.Identity.Users.Get;

public class GetUserRequestHandler<TRequest, TUserDetailsDto> : IRequestHandler<TRequest, TUserDetailsDto>
    where TRequest : GetUserRequest<TUserDetailsDto>
    where TUserDetailsDto : UserDetailsDto
{
    private readonly IUserService _userService;

    protected GetUserRequestHandler(IUserService userService) => _userService = userService;

    public Task<TUserDetailsDto> Handle(TRequest request, CancellationToken cancellationToken)
        => _userService.GetAsync<TUserDetailsDto>(request.Id, cancellationToken);
}

public class GetUserRequestHandler : GetUserRequestHandler<GetUserRequest, UserDetailsDto>
{
    public GetUserRequestHandler(IUserService userService)
        : base(userService)
    {
    }
}

public class GetCustomerRequestHandler : GetUserRequestHandler<GetCustomerRequest, CustomerDetailsDto>
{
    public GetCustomerRequestHandler(IUserService userService)
        : base(userService)
    {
    }
}

public class GetEmployeeRequestHandler : GetUserRequestHandler<GetEmployeeRequest, EmployeeDetailsDto>
{
    public GetEmployeeRequestHandler(IUserService userService)
        : base(userService)
    {
    }
}