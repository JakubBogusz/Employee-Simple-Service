using API.Controllers;
using API.Data.Interfaces;
using API.Dtos;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IGenericRepository<Employee>> _mockEmployeeRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
    
        public EmployeeControllerTests()
        {
            _mockEmployeeRepository = new Mock<IGenericRepository<Employee>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
        }
    
        [Fact]
        public async Task GetAllEmployees_ReturnsOkResult()
        {
            // Arrange
            var employees = CreateEmployeeList();
            var employeeDtos = CreateEmployeeDtosList();
            
            _mockEmployeeRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(employees);
            _mockMapper.Setup(m => m.Map<IReadOnlyList<EmployeeDto>>(employees))
                .Returns(employeeDtos);
            
            var controller = new EmployeesController(_mockEmployeeRepository.Object,
                _mockUnitOfWork.Object, _mockMapper.Object);
        
            // Act
            var result = await controller.GetAllEmployees();
        
            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = result as OkObjectResult;
            Assert.Equal(employeeDtos, objectResult.Value);
        }
        
        [Fact]
        public async Task GetEmployeeById_ReturnsOkResult()
        {
            // Arrange
            var employeeId = 1;
            var employees = CreateEmployeeList();
            var employeeDtos = CreateEmployeeDtosList();
            
            _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId))
                .ReturnsAsync(employees[0]);
            _mockMapper.Setup(m => m.Map<EmployeeDto>(employees[0]))
                .Returns(employeeDtos[0]);
            
            var controller = new EmployeesController(_mockEmployeeRepository.Object,
                _mockUnitOfWork.Object, _mockMapper.Object);
        
            // Act
            var result = await controller.GetEmployeeById(employeeId);
        
            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = result as OkObjectResult;
            Assert.Equal(employeeDtos[0], objectResult.Value);
        }
        
        [Fact]
        public async Task AddEmployee_WhenEmployeeDtoDataIsCorrect_ReturnsOkResult()
        {
            // Arrange
            var employees = CreateEmployeeList();
            var employeeDtos = CreateEmployeeDtosList();
            var addedEmployeeEntity = employees[0];
            var employeeFromDto = employeeDtos[0];
            
            _mockMapper.Setup(x => x.Map<Employee>(employeeFromDto)).Returns(addedEmployeeEntity);
            
            _mockUnitOfWork.Setup(x => x.Repository<Employee>().Add(addedEmployeeEntity));
            _mockUnitOfWork.Setup(x => x.Complete()).ReturnsAsync(1);

            var controller = new EmployeesController(_mockEmployeeRepository.Object,
                _mockUnitOfWork.Object, _mockMapper.Object);

            // Act
            var result = await controller.AddEmployee(employeeFromDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(addedEmployeeEntity, (result as OkObjectResult).Value);
            _mockMapper.Verify(x => x.Map<Employee>(employeeFromDto), Times.Once);
            _mockUnitOfWork.Verify(x => x.Repository<Employee>().Add(addedEmployeeEntity), Times.Once);
            _mockUnitOfWork.Verify(x => x.Complete(), Times.Once);
        }
        
        [Fact]
        public async Task AddEmployee_WhenEmployeeIsNull_ReturnsNotFoundResult()
        {
            // Arrange
            var employeeDto = new EmployeeDto { Id = 1, FirstName = "John", LastName = "Doe"};
            
            _mockMapper.Setup(x => x.Map<Employee>(employeeDto)).Returns((Employee)null);

            var controller = new EmployeesController(_mockEmployeeRepository.Object, 
                _mockUnitOfWork.Object, _mockMapper.Object);

            // Act
            var result = await controller.AddEmployee(employeeDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockMapper.Verify(x => x.Map<Employee>(employeeDto), Times.Once);
            _mockUnitOfWork.Verify(x => x.Repository<Employee>().Add(It.IsAny<Employee>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.Complete(), Times.Never);
        }
        
        [Fact]
        public async Task UpdateEmployee_WhenEmployeeDtoFound_ReturnsOkResult()
        {
            // Arrange
            var employeeToUpdate = new Employee { Id = 1, FirstName = "John" };
            var employees = CreateEmployeeList();
            var employeeDtos = CreateEmployeeDtosList();
            var updatedEmployeeEntity = employees[0];
            var employeeFromDto = employeeDtos[0];
            
            _mockUnitOfWork.Setup(uow => uow.Repository<Employee>().GetByIdAsync(employeeFromDto.Id)).ReturnsAsync(employeeToUpdate);
            
            _mockMapper.Setup(x => x.Map(It.IsAny<EmployeeDto>(), It.IsAny<Employee>())).Returns(updatedEmployeeEntity);
            
            _mockUnitOfWork.Setup(x => x.Repository<Employee>().Update(updatedEmployeeEntity));
            _mockUnitOfWork.Setup(x => x.Complete()).ReturnsAsync(1);

            var controller = new EmployeesController(_mockEmployeeRepository.Object,
                _mockUnitOfWork.Object, _mockMapper.Object);

            // Act
            var result = await controller.UpdateEmployee(employeeFromDto.Id, employeeFromDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedEmployeeEntity, (result as OkObjectResult).Value);
            _mockMapper.Verify(x => x.Map(It.IsAny<EmployeeDto>(), It.IsAny<Employee>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.Repository<Employee>().Update(updatedEmployeeEntity), Times.Once);
            _mockUnitOfWork.Verify(x => x.Complete(), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployee_WhenEmployeeIsNull_ReturnsNotFoundResult()
        {
            // Arrange
            var employeeDto = new EmployeeDto { Id = 1, FirstName = "John", LastName = "Doe"};
            
            _mockUnitOfWork.Setup(uow => uow.Repository<Employee>().GetByIdAsync(employeeDto.Id)).ReturnsAsync((Employee)null);
            
            var controller = new EmployeesController(_mockEmployeeRepository.Object, 
                _mockUnitOfWork.Object, _mockMapper.Object);

            // Act
            var result = await controller.UpdateEmployee(employeeDto.Id, employeeDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockUnitOfWork.Verify(x => x.Repository<Employee>().GetByIdAsync(It.IsAny<int>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.Repository<Employee>().Update(It.IsAny<Employee>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.Complete(), Times.Never);
        }
        
        [Fact]
        public async Task DeleteEmployee_DeletesEmployee_AndReturnsOkResult()
        {
            // Arrange
            var employee = CreateEmployeeList();
            var employeeToRemove = employee[0];
            
            _mockEmployeeRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(employeeToRemove);
            
            _mockUnitOfWork.Setup(uow => uow.Repository<Employee>().Delete(It.IsAny<Employee>()));
            _mockUnitOfWork.Setup(uow => uow.Complete()).ReturnsAsync(1);

            var controller = new EmployeesController(_mockEmployeeRepository.Object,
                _mockUnitOfWork.Object, _mockMapper.Object);

            // Act
            var result = await controller.DeleteEmployee(employeeToRemove.Id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Same(employeeToRemove, ((OkObjectResult)result).Value);
            _mockUnitOfWork.Verify(uow => uow.Repository<Employee>().Delete(employeeToRemove), Times.Once());
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Once());
        }
        
        [Fact]
        public async Task DeleteEmployee_WhenEmployeeIsNull_ReturnsNotFound()
        {
            // Arrange
            _mockEmployeeRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Employee)null);
            
            _mockUnitOfWork.Setup(uow => uow.Repository<Employee>().Delete(It.IsAny<Employee>()));
            _mockUnitOfWork.Setup(uow => uow.Complete()).ReturnsAsync(1);

            var controller = new EmployeesController(_mockEmployeeRepository.Object,
                _mockUnitOfWork.Object, _mockMapper.Object);

            // Act
            var result = await controller.DeleteEmployee(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        
        private List<Employee> CreateEmployeeList()
        {
            return new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    StreetName = "Main Street",
                    HouseNumber = "1",
                    ApartmentNumber = null,
                    PostalCode = "12345",
                    Town = "New York",
                    PhoneNumber = "123-456-7890",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Age = 32
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    StreetName = "Maple Avenue",
                    HouseNumber = "2",
                    ApartmentNumber = 1,
                    PostalCode = "23456",
                    Town = "Chicago",
                    PhoneNumber = "234-567-8901",
                    DateOfBirth = new DateTime(1985, 2, 14),
                    Age = 37
                }
            };
        }

        private List<EmployeeDto> CreateEmployeeDtosList()
        {
            return new List<EmployeeDto>
            {
                new EmployeeDto
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    StreetName = "Main Street",
                    HouseNumber = "1",
                    ApartmentNumber = null,
                    PostalCode = "12345",
                    Town = "New York",
                    PhoneNumber = "123-456-7890",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Age = 32
                },
                new EmployeeDto
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    StreetName = "Maple Avenue",
                    HouseNumber = "2",
                    ApartmentNumber = 1,
                    PostalCode = "23456",
                    Town = "Chicago",
                    PhoneNumber = "234-567-8901",
                    DateOfBirth = new DateTime(1985, 2, 14),
                    Age = 37
                }
            };
        }
    }
}