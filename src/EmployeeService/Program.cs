using EmployeeService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// Database Configuration
// ==========================================
builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ==========================================
// Add Controllers
// ==========================================
builder.Services.AddControllers();

// ==========================================
// OpenAPI
// ==========================================
builder.Services.AddOpenApi();

// ==========================================
// Department Service HTTP Client
// DepartmentService runs on port 5079
// ==========================================
builder.Services.AddHttpClient("DepartmentService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5079");
});

// ==========================================
// CORS Configuration
// Allow React Frontend on port 5173
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ==========================================
// Development Configuration
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "Employee Service API"
        );
    });
}

// ==========================================
// CORS Middleware
// IMPORTANT: This connects React frontend
// with EmployeeService
// ==========================================
app.UseCors("AllowFrontend");

// ==========================================
// HTTPS Redirection
// ==========================================
app.UseHttpsRedirection();

// ==========================================
// Authorization
// ==========================================
app.UseAuthorization();

// ==========================================
// Controller Endpoints
// ==========================================
app.MapControllers();

// ==========================================
// Start Application
// ==========================================
app.Run();