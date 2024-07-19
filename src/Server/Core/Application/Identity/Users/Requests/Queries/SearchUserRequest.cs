using MultiMart.Application.Common.Models;
using MultiMart.Application.Identity.Users.Interfaces;
using MultiMart.Application.Identity.Users.Models;

namespace MultiMart.Application.Identity.Users.Requests.Queries;

public class SearchUserRequest : UserListFilter, IRequest<PaginationResponse<UserDetailsDto>>
{
}

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