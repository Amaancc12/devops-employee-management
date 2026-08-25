using DepartmentService.Data;
using DepartmentService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DepartmentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly DepartmentDbContext _context;

    public DepartmentsController(DepartmentDbContext context)
    {
        _context = context;
    }

    // GET: api/Departments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
    {
        return await _context.Departments.ToListAsync();
    }

    // GET: api/Departments/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Department>> GetDepartment(int id)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null)
        {
            return NotFound();
        }

        return Ok(department);
    }

    // POST: api/Departments
    [HttpPost]
    public async Task<ActionResult<Department>> CreateDepartment(
        Department department)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetDepartment),
            new { id = department.Id },
            department);
    }

    // PUT: api/Departments/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Department>> UpdateDepartment(
        int id,
        Department updatedDepartment)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null)
        {
            return NotFound();
        }

        department.Name = updatedDepartment.Name;
        department.Description = updatedDepartment.Description;

        await _context.SaveChangesAsync();

        return Ok(department);
    }

    // DELETE: api/Departments/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null)
        {
            return NotFound();
        }

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}