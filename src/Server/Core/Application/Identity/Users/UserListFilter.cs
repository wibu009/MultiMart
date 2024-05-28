using MultiMart.Application.Common.Models;

namespace MultiMart.Application.Identity.Users;

public class UserListFilter : PaginationFilter
{
    public bool? IsActive { get; set; }
}