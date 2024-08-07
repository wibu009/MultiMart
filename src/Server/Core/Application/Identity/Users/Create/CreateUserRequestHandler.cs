namespace MultiMart.Application.Identity.Users.Create;

public class CreateUserRequestHandler<TCreateUserRequest> : IRequestHandler<TCreateUserRequest, string>
    where TCreateUserRequest : CreateUserRequest
{
    private readonly IUserService _userService;
    private readonly IStringLocalizer _t;

    protected CreateUserRequestHandler(IUserService userService, IStringLocalizer<CreateUserRequestHandler<TCreateUserRequest>> t)
    {
        _userService = userService;
        _t = t;
    }

    public async Task<string> Handle(TCreateUserRequest request, CancellationToken cancellationToken)
    {
        await _userService.CreateAsync(request, cancellationToken);
        return _t["User created successfully."];
    }
}

public class CreateUserRequestHandler : CreateUserRequestHandler<CreateUserRequest>
{
    public CreateUserRequestHandler(IUserService userService, IStringLocalizer<CreateUserRequestHandler> t)
        : base(userService, t)
    {
    }
}

public class CreateCustomerRequestHandler : CreateUserRequestHandler<CreateCustomerRequest>
{
    public CreateCustomerRequestHandler(IUserService userService, IStringLocalizer<CreateCustomerRequestHandler> t)
        : base(userService, t)
    {
    }
}

public class CreateEmployeeRequestHandler : CreateUserRequestHandler<CreateEmployeeRequest>
{
    public CreateEmployeeRequestHandler(IUserService userService, IStringLocalizer<CreateEmployeeRequestHandler> t)
        : base(userService, t)
    {
    }
}
