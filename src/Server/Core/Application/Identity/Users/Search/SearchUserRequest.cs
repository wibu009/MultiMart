namespace MultiMart.Application.Identity.Users.Search;

public class SearchUserRequest<TUserDetailsDto> : PaginationFilter, IRequest<PaginationResponse<TUserDetailsDto>>
    where TUserDetailsDto : UserDetailsDto
{
}

public class SearchUserRequest : SearchUserRequest<UserDetailsDto>
{
}

public class SearchCustomerRequest : SearchUserRequest<CustomerDetailsDto>
{
}

public class SearchEmployeeRequest : SearchUserRequest<EmployeeDetailsDto>
{
}