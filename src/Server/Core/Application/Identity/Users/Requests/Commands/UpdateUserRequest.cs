using System.Text.Json.Serialization;
using MultiMart.Application.Common.FileStorage;
using MultiMart.Application.Identity.Users.Interfaces;
using MultiMart.Domain.Common.Enums;

namespace MultiMart.Application.Identity.Users.Requests.Commands;

public class UpdateUserRequest : IRequest<Unit>
{
    [JsonIgnore]
    public string Id { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public FileUploadRequest? Image { get; set; }
    public bool DeleteCurrentImage { get; set; } = false;
}

public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest>
{
    private readonly IUserService _userService;

    public UpdateUserRequestHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Unit> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        await _userService.UpdateAsync(request, cancellationToken);

        return Unit.Value;
    }
}