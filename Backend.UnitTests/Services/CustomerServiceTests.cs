using FluentAssertions;
using Moq;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Application.Services;
using OrderManagementSystem.Models;

namespace Backend.UnitTests.Services;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _repositoryMock;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _repositoryMock = new Mock<ICustomerRepository>();
        _service = new CustomerService(_repositoryMock.Object);
    }

    // ---------------- GET ALL ----------------
    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedCustomers()
    {
        // Arrange
        var fakeCustomers = new List<Customer>
        {
            new Customer
            {
                CustomerID = "ALFKI",
                CompanyName = "Alfreds Futterkiste",
                City = "Berlin",
                Country = "Germany"
            },
            new Customer
            {
                CustomerID = "ANATR",
                CompanyName = "Ana Trujillo",
                City = "Mexico City",
                Country = "Mexico"
            }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(fakeCustomers);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);

        result[0].CustomerID.Should().Be("ALFKI");
        result[0].CompanyName.Should().Be("Alfreds Futterkiste");
        result[0].City.Should().Be("Berlin");
        result[0].Country.Should().Be("Germany");
    }

    // ---------------- GET BY ID (FOUND) ----------------
    [Fact]
    public async Task GetByIdAsync_ShouldReturnCustomer_WhenExists()
    {
        // Arrange
        string id = "ALFKI";

        var fakeCustomer = new Customer
        {
            CustomerID = id,
            CompanyName = "Alfreds Futterkiste",
            City = "Berlin",
            Country = "Germany"
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(fakeCustomer);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result.CustomerID.Should().Be(id);
        result.CompanyName.Should().Be("Alfreds Futterkiste");
    }

    // ---------------- GET BY ID (NOT FOUND) ----------------
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
    {
        // Arrange
        string id = "UNKNOWN";

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }
}