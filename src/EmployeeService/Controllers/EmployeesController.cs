using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeService.Models;
using EmployeeService.Data;

namespace EmployeeService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly EmployeeDbContext _context;
    private readonly HttpClient _httpClient;

    public EmployeesController(
        EmployeeDbContext context,
        IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClient = httpClientFactory.CreateClient("DepartmentService");
    }

// GET: api/Employees

[HttpGet]
public async Task<ActionResult<IEnumerable<object>>> GetEmployees()
{
    var employees = await _context.Employees.ToListAsync();

    var result = new List<object>();

    foreach (var employee in employees)
    {
        var response = await _httpClient.GetAsync(
            $"/api/Departments/{employee.DepartmentID}");

        if (response.IsSuccessStatusCode)
        {
            var department = await response.Content.ReadFromJsonAsync<object>();

            result.Add(new
            {
                employee.Id,
                employee.Name,
                employee.Email,
                employee.Salary,
                employee.DepartmentID,
                Department = department
            });
        }
        else
        {
            result.Add(new
            {
                employee.Id,
                employee.Name,
                employee.Email,
                employee.Salary,
                employee.DepartmentID,
                Department = (object?)null
            });
        }
    }

    return Ok(result);
}

    // GET: api/Employees/1
[HttpGet("{id:int}")]
public async Task<ActionResult> GetEmployee(
    [FromRoute] int id)
{
    var employee = await _context.Employees.FindAsync(id);

    if (employee == null)
    {
        return NotFound();
    }

    // Call DepartmentService
    var response = await _httpClient.GetAsync(
        $"/api/Departments/{employee.DepartmentID}");

    if (!response.IsSuccessStatusCode)
    {
        return Ok(new
        {
            employee.Id,
            employee.Name,
            employee.Email,
            employee.Salary,
            employee.DepartmentID,
            Department = (object?)null
        });
    }

    var department = await response.Content.ReadFromJsonAsync<object>();

    return Ok(new
    {
        employee.Id,
        employee.Name,
        employee.Email,
        employee.Salary,
        employee.DepartmentID,
        Department = department
    });
}

    // POST: api/Employees
    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee(
        [FromBody] Employee employee)
    {
        _context.Employees.Add(employee);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEmployee),
            new { id = employee.Id },
            employee);
    }

    // PUT: api/Employees/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Employee>> UpdateEmployee(
        [FromRoute] int id,
        [FromBody] Employee updatedEmployee)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        employee.Name = updatedEmployee.Name;
        employee.Email = updatedEmployee.Email;
        employee.DepartmentID = updatedEmployee.DepartmentID;
        employee.Salary = updatedEmployee.Salary;

        await _context.SaveChangesAsync();

        return Ok(employee);
    }

    // DELETE: api/Employees/1
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteEmployee(
        [FromRoute] int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        _context.Employees.Remove(employee);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}