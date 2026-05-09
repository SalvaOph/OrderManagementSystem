using FluentAssertions;
using Moq;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Application.Services;
using OrderManagementSystem.Models;

namespace Backend.UnitTests.Services;

public class ShipperServiceTests
{
    private readonly Mock<IShipperRepository> _repoMock;
    private readonly ShipperService _service;

    public ShipperServiceTests()
    {
        _repoMock = new Mock<IShipperRepository>();
        _service = new ShipperService(_repoMock.Object);
    }

    // ---------------- GET ALL ----------------
    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedShippers()
    {
        // Arrange
        var fakeList = new List<Shipper>
        {
            new Shipper
            {
                ShipperID = 1,
                CompanyName = "Fast Express",
                Phone = "123456789"
            },
            new Shipper
            {
                ShipperID = 2,
                CompanyName = "Global Shipping",
                Phone = "987654321"
            }
        };

        _repoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(fakeList);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);

        result[0].ShipperID.Should().Be(1);
        result[0].CompanyName.Should().Be("Fast Express");
        result[0].Phone.Should().Be("123456789");

        result[1].ShipperID.Should().Be(2);
        result[1].CompanyName.Should().Be("Global Shipping");
        result[1].Phone.Should().Be("987654321");
    }

    // ---------------- GET BY ID ----------------
    [Fact]
    public async Task GetByIdAsync_ShouldReturnShipper_WhenExists()
    {
        // Arrange
        int id = 1;

        var fakeEntity = new Shipper
        {
            ShipperID = id,
            CompanyName = "Fast Express",
            Phone = "123456789"
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(fakeEntity);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.ShipperID.Should().Be(id);
        result.CompanyName.Should().Be("Fast Express");
        result.Phone.Should().Be("123456789");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Shipper?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }
}