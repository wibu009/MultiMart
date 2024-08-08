namespace MultiMart.Application.Identity.Users.Create.Employee;

public class CreateEmployeeRequest : CreateUserRequest
{
    public string Position { get; set; } = default!;
    public string Department { get; set; } = default!;
    public DateTime HireDate { get; set; }
    public string? ManagerId { get; set; }
}