using FluentAssertions;
using Moq;
using OrderManagementSystem.Application.Dtos.Employee;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Application.Services;
using OrderManagementSystem.Models;
using Xunit;

namespace Backend.UnitTests.Services;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _repositoryMock;
    private readonly EmployeeService _service;

    public EmployeeServiceTests()
    {
        _repositoryMock = new Mock<IEmployeeRepository>();
        _service = new EmployeeService(_repositoryMock.Object);
    }

    // ---------------- GET ALL ----------------
    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedEmployees()
    {
        // Arrange
        var fakeEmployees = new List<Employee>
        {
            new Employee
            {
                EmployeeID = 1,
                FirstName = "Nancy",
                LastName = "Davolio",
                Title = "Sales Representative"
            },
            new Employee
            {
                EmployeeID = 2,
                FirstName = "Andrew",
                LastName = "Fuller",
                Title = "Vice President"
            }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(fakeEmployees);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);

        result[0].EmployeeID.Should().Be(1);
        result[0].FirstName.Should().Be("Nancy");
        result[0].LastName.Should().Be("Davolio");
        result[0].Title.Should().Be("Sales Representative");
    }

    // ---------------- GET BY ID (FOUND) ----------------
    [Fact]
    public async Task GetByIdAsync_ShouldReturnEmployee_WhenExists()
    {
        // Arrange
        int id = 1;

        var fakeEmployee = new Employee
        {
            EmployeeID = id,
            FirstName = "Nancy",
            LastName = "Davolio",
            Title = "Sales Representative"
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(fakeEmployee);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.EmployeeID.Should().Be(id);
        result.FirstName.Should().Be("Nancy");
        result.LastName.Should().Be("Davolio");
        result.Title.Should().Be("Sales Representative");
    }

    // ---------------- GET BY ID (NOT FOUND) ----------------
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenEmployeeDoesNotExist()
    {
        // Arrange
        int id = 999;

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Employee?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }
}