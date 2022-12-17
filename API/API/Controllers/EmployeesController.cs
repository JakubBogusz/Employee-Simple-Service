using API.Data.Interfaces;
using API.Dtos;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : Controller
{
    private readonly IGenericRepository<Employee> _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EmployeesController(IGenericRepository<Employee> employeeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _employeeRepository.GetAllAsync();

        return Ok(_mapper.Map<IReadOnlyList<EmployeeDto>>(employees));
    }
    
    [HttpGet]
    [Route("{employeeId:int}")]
    public async Task<IActionResult> GetEmployeeById(int employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);

        if (employee == null)
        {
            return NotFound();
        }
        
        return Ok(_mapper.Map<IReadOnlyList<EmployeeDto>>(employee));
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
    {
        _unitOfWork.Repository<Employee>().Add(employee);
        await _unitOfWork.Complete();
        
        return Ok(employee);
    }
    
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateEmployee([FromRoute] int id, Employee updateEmployee)
    {
        var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);
        
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

        _unitOfWork.Repository<Employee>().Update(employee);
        await _unitOfWork.Complete();

        return Ok(employee);
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }
        
        _unitOfWork.Repository<Employee>().Delete(employee);
        await _unitOfWork.Complete();

        return Ok(employee);
    }
}