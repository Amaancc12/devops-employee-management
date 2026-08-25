using Microsoft.EntityFrameworkCore;
using DepartmentService.Models;

namespace DepartmentService.Data;

public class DepartmentDbContext : DbContext
{
    public DepartmentDbContext(
        DbContextOptions<DepartmentDbContext> options)
        : base(options)
    {
    }

    public DbSet<Department> Departments { get; set; }
}