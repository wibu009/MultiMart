namespace MultiMart.Application.Identity.Users.Search;

public class SearchUserRequest : PaginationFilter, IRequest<PaginationResponse<UserDetailsDto>>
{
}