using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : Controller
{
    private readonly ApplicationDbContext _applicationDbContext;

    public EmployeesController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _applicationDbContext.Employees.ToListAsync();

        return Ok(employees);
    }
    
    [HttpGet]
    [Route("{employeeId:Guid}")]
    public async Task<IActionResult> GetEmployeeById(Guid employeeId)
    {
        var employee = await _applicationDbContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);

        if (employee == null)
        {
            return NotFound();
        }
    
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
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, Employee updateEmployee)
    {
        var employee = await _applicationDbContext.Employees.FindAsync(id);
        
        if (employee == null)
        {
            return NotFound();
        }

        employee.FirstName = updateEmployee.FirstName;
        employee.LastName = updateEmployee.LastName;
        employee.Town = updateEmployee.Town;
        employee.StreetName = updateEmployee.StreetName;
        employee.ApartmentNumber = updateEmployee.ApartmentNumber;
        employee.HouseNumber = updateEmployee.HouseNumber;
        employee.DateOfBirth = updateEmployee.DateOfBirth;
        employee.PostalCode = updateEmployee.PostalCode;
        employee.PhoneNumber = updateEmployee.PhoneNumber;
        employee.Age = updateEmployee.Age;

        await _applicationDbContext.SaveChangesAsync();

        return Ok(employee);
    }
    
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
    {
        var employee = await _applicationDbContext.Employees.FindAsync(id);

        if (employee == null)
        {
            return NotFound();
        }
        
        _applicationDbContext.Employees.Remove(employee);
        await _applicationDbContext.SaveChangesAsync();

        return Ok(employee);
    }
}