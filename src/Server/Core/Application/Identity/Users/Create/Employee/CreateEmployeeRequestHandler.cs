namespace MultiMart.Application.Identity.Users.Create.Employee;

public class CreateEmployeeRequestHandler : CreateUserRequestHandler<CreateEmployeeRequest>
{
    public CreateEmployeeRequestHandler(IUserService userService, IStringLocalizer<CreateEmployeeRequestHandler> t)
        : base(userService, t)
    {
    }
}