using MultiMart.Application.Identity.Users.Interfaces;

namespace MultiMart.Application.Identity.Users.Requests;

public class GetUserPermissionsRequest : IRequest<List<string>>
{
    public string Id { get; set; }

    public GetUserPermissionsRequest(string id)
    {
        Id = id;
    }
}

public class GetUserPermissionsRequestHandler : IRequestHandler<GetUserPermissionsRequest, List<string>>
{
    private readonly IUserService _userService;

    public GetUserPermissionsRequestHandler(IUserService userService) => _userService = userService;

    public Task<List<string>> Handle(GetUserPermissionsRequest request, CancellationToken cancellationToken)
        => _userService.GetPermissionsAsync(request.Id, cancellationToken);
}