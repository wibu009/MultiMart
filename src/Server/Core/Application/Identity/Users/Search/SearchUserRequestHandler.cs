namespace MultiMart.Application.Identity.Users.Search;

public class SearchUserRequestHandler<TRequest, TUserDetailsDto> : IRequestHandler<TRequest, PaginationResponse<TUserDetailsDto>>
    where TRequest : SearchUserRequest<TUserDetailsDto>
    where TUserDetailsDto : UserDetailsDto
{
    private readonly IUserService _userService;

    public SearchUserRequestHandler(IUserService userService) => _userService = userService;

    public async Task<PaginationResponse<TUserDetailsDto>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        return await _userService.SearchAsync<TUserDetailsDto, TRequest>(request, cancellationToken);
    }
}

public class SearchCustomerRequestHandler : SearchUserRequestHandler<SearchCustomerRequest, CustomerDetailsDto>
{
    public SearchCustomerRequestHandler(IUserService userService)
        : base(userService)
    {
    }
}

public class SearchEmployeeRequestHandler : SearchUserRequestHandler<SearchEmployeeRequest, EmployeeDetailsDto>
{
    public SearchEmployeeRequestHandler(IUserService userService)
        : base(userService)
    {
    }
}