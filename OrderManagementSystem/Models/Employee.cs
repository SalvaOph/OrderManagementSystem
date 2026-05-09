namespace OrderManagementSystem.Models;

public class Employee
{
    public int EmployeeID { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    
}