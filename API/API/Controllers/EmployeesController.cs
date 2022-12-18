using API.Data.Interfaces;
using API.Dtos;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// This project is still under development
// In the future I'm going to add better exception handeling and logging mechanism

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
        
        if (employees.Count < 1)
        {
            return NotFound();
        }

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
        
        return Ok(_mapper.Map<EmployeeDto>(employee));
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto)
    {
        var employee = _mapper.Map<Employee>(employeeDto);
        
        if (employee == null)
        {
            return NotFound();
        }
        
        _unitOfWork.Repository<Employee>().Add(employee);
        await _unitOfWork.Complete();
        
        return Ok(employee);
    }
    
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateEmployee([FromRoute] int id, EmployeeDto updateEmployeeDto)
    {
        var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);
        
        if (employee == null)
        {
            return NotFound();
        }

        var updatedEmployee = _mapper.Map(updateEmployeeDto, employee);
        
        _unitOfWork.Repository<Employee>().Update(updatedEmployee);
        await _unitOfWork.Complete();

        return Ok(updatedEmployee);
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