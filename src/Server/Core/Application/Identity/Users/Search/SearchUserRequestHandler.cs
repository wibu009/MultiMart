namespace MultiMart.Application.Identity.Users.Search;

public class SearchUserRequestHandler : IRequestHandler<SearchUserRequest, PaginationResponse<UserDetailsDto>>
{
    private readonly IUserService _userService;

    public SearchUserRequestHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<PaginationResponse<UserDetailsDto>> Handle(SearchUserRequest request, CancellationToken cancellationToken)
    {
        return await _userService.SearchAsync(request, cancellationToken);
    }
}