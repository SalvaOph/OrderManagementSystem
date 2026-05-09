using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementSystem.Application.Dtos.Shipper;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Controllers;

namespace Backend.UnitTests.Controllers;

public class ShippersControllerTests
{
    private readonly Mock<IShipperService> _serviceMock;
    private readonly ShippersController _controller;

    public ShippersControllerTests()
    {
        _serviceMock = new Mock<IShipperService>();

        _controller = new ShippersController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithShippers()
    {
        // Arrange
        var fakeShippers = new List<ShipperDto>
        {
            new ShipperDto
            {
                ShipperID = 1,
                CompanyName = "Speedy Express",
                Phone = "123-456-789"
            },
            new ShipperDto
            {
                ShipperID = 2,
                CompanyName = "United Package",
                Phone = "987-654-321"
            }
        };

        _serviceMock
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(fakeShippers);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value.Should().BeEquivalentTo(fakeShippers);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenShipperExists()
    {
        // Arrange
        int shipperId = 1;

        var fakeShipper = new ShipperDto
        {
            ShipperID = shipperId,
            CompanyName = "Speedy Express",
            Phone = "123-456-789"
        };

        _serviceMock
            .Setup(s => s.GetByIdAsync(shipperId))
            .ReturnsAsync(fakeShipper);

        // Act
        var result = await _controller.GetById(shipperId);

        // Assert
        var okResult = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value.Should().BeEquivalentTo(fakeShipper);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenShipperDoesNotExist()
    {
        // Arrange
        int shipperId = 999;

        _serviceMock
            .Setup(s => s.GetByIdAsync(shipperId))
            .ReturnsAsync((ShipperDto?)null);

        // Act
        var result = await _controller.GetById(shipperId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}