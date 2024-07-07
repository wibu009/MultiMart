using MultiMart.Application.Identity.Users.Interfaces;
using MultiMart.Application.Identity.Users.Models;

namespace MultiMart.Application.Identity.Users.Requests.Queries;

public class GetAllUserRequest : IRequest<List<UserDetailsDto>>
{
}

public class GetAllUserRequestHandler : IRequestHandler<GetAllUserRequest, List<UserDetailsDto>>
{
    private readonly IUserService _userService;

    public GetAllUserRequestHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<List<UserDetailsDto>> Handle(GetAllUserRequest request, CancellationToken cancellationToken)
    {
        return await _userService.GetListAsync(cancellationToken);
    }
}