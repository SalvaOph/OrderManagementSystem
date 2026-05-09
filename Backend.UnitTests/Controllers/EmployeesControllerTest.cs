using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementSystem.Application.Dtos.Employee;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Controllers;

namespace Backend.UnitTests.Controllers;

public class EmployeesControllerTests
{
    private readonly Mock<IEmployeeService> _serviceMock;
    private readonly EmployeesController _controller;

    public EmployeesControllerTests()
    {
        _serviceMock = new Mock<IEmployeeService>();

        _controller = new EmployeesController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithEmployees()
    {
        // Arrange
        var fakeEmployees = new List<EmployeeDto>
        {
            new EmployeeDto
            {
                EmployeeID = 1,
                FirstName = "Nancy",
                LastName = "Davolio",
                Title = "Sales Representative"
            },
            new EmployeeDto
            {
                EmployeeID = 2,
                FirstName = "Andrew",
                LastName = "Fuller",
                Title = "Vice President"
            }
        };

        _serviceMock
            .Setup(service => service.GetAllAsync())
            .ReturnsAsync(fakeEmployees);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value.Should().BeEquivalentTo(fakeEmployees);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenEmployeeExists()
    {
        // Arrange
        int employeeId = 1;

        var fakeEmployee = new EmployeeDto
        {
            EmployeeID = employeeId,
            FirstName = "Nancy",
            LastName = "Davolio",
            Title = "Sales Representative"
        };

        _serviceMock
            .Setup(service => service.GetByIdAsync(employeeId))
            .ReturnsAsync(fakeEmployee);

        // Act
        var result = await _controller.GetById(employeeId);

        // Assert
        var okResult = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value.Should().BeEquivalentTo(fakeEmployee);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
    {
        // Arrange
        int employeeId = 999;

        _serviceMock
            .Setup(service => service.GetByIdAsync(employeeId))
            .ReturnsAsync((EmployeeDto?)null);

        // Act
        var result = await _controller.GetById(employeeId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}