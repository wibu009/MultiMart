namespace MultiMart.Application.Identity.Users.Get;

public class GetUserRequest<TUserDetailsDto> : IRequest<TUserDetailsDto>
    where TUserDetailsDto : UserDetailsDto
{
    public string Id { get; set; }

    protected GetUserRequest(string id)
    {
        Id = id;
    }
}

public class GetUserRequest : GetUserRequest<UserDetailsDto>
{
    public GetUserRequest(string id)
        : base(id)
    {
    }
}

public class GetCustomerRequest : GetUserRequest<CustomerDetailsDto>
{
    public GetCustomerRequest(string id)
        : base(id)
    {
    }
}

public class GetEmployeeRequest : GetUserRequest<EmployeeDetailsDto>
{
    public GetEmployeeRequest(string id)
        : base(id)
    {
    }
}