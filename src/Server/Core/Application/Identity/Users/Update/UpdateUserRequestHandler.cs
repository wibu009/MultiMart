namespace MultiMart.Application.Identity.Users.Update;

public class UpdateUserRequestHandler<TUpdateUserRequest> : IRequestHandler<TUpdateUserRequest, string>
    where TUpdateUserRequest : UpdateUserRequest
{
    private readonly IUserService _userService;
    private readonly IStringLocalizer _t;

    public UpdateUserRequestHandler(IUserService userService, IStringLocalizer<UpdateUserRequestHandler<TUpdateUserRequest>> t)
    {
        _userService = userService;
        _t = t;
    }

    public async Task<string> Handle(TUpdateUserRequest request, CancellationToken cancellationToken)
    {
        await _userService.UpdateAsync(request, cancellationToken);
        return _t["User updated successfully."];
    }
}

public class UpdateCustomerRequestHandler : UpdateUserRequestHandler<UpdateCustomerRequest>
{
    public UpdateCustomerRequestHandler(IUserService userService, IStringLocalizer<UpdateCustomerRequestHandler> t)
        : base(userService, t)
    {
    }
}

public class UpdateEmployeeRequestHandler : UpdateUserRequestHandler<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestHandler(IUserService userService, IStringLocalizer<UpdateEmployeeRequestHandler> t)
        : base(userService, t)
    {
    }
}