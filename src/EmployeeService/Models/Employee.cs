namespace EmployeeService.Models;

public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int DepartmentID { get; set; }

    public decimal Salary { get; set; }
}