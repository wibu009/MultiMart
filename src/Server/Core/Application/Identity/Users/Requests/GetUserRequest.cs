using MultiMart.Application.Identity.Users.Interfaces;
using MultiMart.Application.Identity.Users.Models;

namespace MultiMart.Application.Identity.Users.Requests;

public class GetUserRequest : IRequest<UserDetailsDto>
{
    public string Id { get; set; }

    public GetUserRequest(string id)
    {
        Id = id;
    }
}

public class GetUserRequestHandler : IRequestHandler<GetUserRequest, UserDetailsDto>
{
    private readonly IUserService _userService;

    public GetUserRequestHandler(IUserService userService) => _userService = userService;

    public Task<UserDetailsDto> Handle(GetUserRequest request, CancellationToken cancellationToken)
        => _userService.GetAsync(request.Id, cancellationToken);
}