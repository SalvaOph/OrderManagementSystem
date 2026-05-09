using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementSystem.Application.Dtos.Customer;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Controllers;

namespace Backend.UnitTests.Controllers;

public class CustomerControllerTests
{
    private readonly Mock<ICustomerService> _serviceMock;
    private readonly CustomerController _controller;

    public CustomerControllerTests()
    {
        _serviceMock = new Mock<ICustomerService>();

        _controller = new CustomerController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetCustomers_ShouldReturnOk_WithCustomers()
    {
        // Arrange
        var fakeCustomers = new List<CustomerDto>
        {
            new CustomerDto
            {
                CustomerID = "ALFKI",
                CompanyName = "Alfreds Futterkiste",
                City = "Berlin",
                Country = "Germany"
            },
            new CustomerDto
            {
                CustomerID = "ANATR",
                CompanyName = "Ana Trujillo Emparedados",
                City = "Mexico City",
                Country = "Mexico"
            }
        };

        _serviceMock
            .Setup(service => service.GetAllAsync())
            .ReturnsAsync(fakeCustomers);

        // Act
        var result = await _controller.GetCustomers();

        // Assert
        var okResult = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value.Should().BeEquivalentTo(fakeCustomers);
    }

    [Fact]
    public async Task GetCustomerById_ShouldReturnOk_WhenCustomerExists()
    {
        // Arrange
        string customerId = "ALFKI";

        var fakeCustomer = new CustomerDto
        {
            CustomerID = customerId,
            CompanyName = "Alfreds Futterkiste",
            City = "Berlin",
            Country = "Germany"
        };

        _serviceMock
            .Setup(service => service.GetByIdAsync(customerId))
            .ReturnsAsync(fakeCustomer);

        // Act
        var result = await _controller.GetCustomerById(customerId);

        // Assert
        var okResult = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value.Should().BeEquivalentTo(fakeCustomer);
    }

    [Fact]
    public async Task GetCustomerById_ShouldReturnNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        string customerId = "UNKNOWN";

        _serviceMock
            .Setup(service => service.GetByIdAsync(customerId))
            .ReturnsAsync((CustomerDto?)null);

        // Act
        var result = await _controller.GetCustomerById(customerId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}