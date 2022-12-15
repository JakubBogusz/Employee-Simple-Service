using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _applicationDbContext;

    public EmployeeController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _applicationDbContext.Employees.ToListAsync();

        return Ok(employees);
    }
    
    [HttpGet("{employeeId}")]
    public async Task<IActionResult> GetEmployeeById(Guid employeeId)
    {
        var employee = await _applicationDbContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);
    
        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
    {
        employee.Id = Guid.NewGuid();
        await _applicationDbContext.Employees.AddAsync(employee);
        await _applicationDbContext.SaveChangesAsync();

        return Ok(employee);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
    {
        _applicationDbContext.Employees.Update(employee);
        await _applicationDbContext.SaveChangesAsync();
    
        return Ok(employee);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteEmployee(Employee employee)
    {
        _applicationDbContext.Employees.Remove(employee);
        await _applicationDbContext.SaveChangesAsync();

        return Ok(employee);
    }
    
    
}