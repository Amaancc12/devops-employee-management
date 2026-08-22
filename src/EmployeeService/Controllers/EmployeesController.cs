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

    public EmployeesController(EmployeeDbContext context)
    {
        _context = context;
    }

    // GET: api/Employees
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        var employees = await _context.Employees.ToListAsync();

        return Ok(employees);
    }

    // GET: api/Employees/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Employee>> GetEmployee([FromRoute] int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
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
        employee.Department = updatedEmployee.Department;
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