namespace OrderManagementSystem.Application.Dtos.Employee;

public class EmployeeDto
{
    public int EmployeeID { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Title { get; set; } = null!;
}