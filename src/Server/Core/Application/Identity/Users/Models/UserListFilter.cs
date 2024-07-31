namespace MultiMart.Application.Identity.Users.Models;

public class UserListFilter : PaginationFilter
{
    public bool? IsActive { get; set; }
}