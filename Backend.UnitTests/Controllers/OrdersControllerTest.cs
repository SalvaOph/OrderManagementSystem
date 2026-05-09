using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementSystem.Application.Dtos.Order;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Controllers;

namespace Backend.UnitTests.Controllers;

public class OrdersControllerTests
{
    private readonly Mock<IOrderService> _serviceMock;
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        _serviceMock = new Mock<IOrderService>();

        _controller = new OrdersController(_serviceMock.Object);
    }

    // ---------------- GET ALL ----------------
    [Fact]
    public async Task GetOrders_ShouldReturnOk_WithOrders()
    {
        // Arrange
        var fakeOrders = new List<OrderDto>
        {
            new OrderDto
            {
                OrderID = 1,
                CustomerID = "ALFKI",
                EmployeeID = 1,
                ShipVia = 2,
                ShipCity = "Berlin"
            },
            new OrderDto
            {
                OrderID = 2,
                CustomerID = "ANATR",
                EmployeeID = 2,
                ShipVia = 1,
                ShipCity = "Mexico City"
            }
        };

        _serviceMock
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(fakeOrders);

        // Act
        var result = await _controller.GetOrders();

        // Assert
        var okResult = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value.Should().BeEquivalentTo(fakeOrders);
    }

    // ---------------- GET BY ID ----------------
    [Fact]
    public async Task GetOrderById_ShouldReturnOk_WhenOrderExists()
    {
        // Arrange
        int orderId = 1;

        var fakeOrder = new OrderDto
        {
            OrderID = orderId,
            CustomerID = "ALFKI",
            EmployeeID = 1,
            ShipCity = "Berlin"
        };

        _serviceMock
            .Setup(s => s.GetByIdAsync(orderId))
            .ReturnsAsync(fakeOrder);

        // Act
        var result = await _controller.GetOrderById(orderId);

        // Assert
        var okResult = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value.Should().BeEquivalentTo(fakeOrder);
    }

    [Fact]
    public async Task GetOrderById_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        int orderId = 999;

        _serviceMock
            .Setup(s => s.GetByIdAsync(orderId))
            .ReturnsAsync((OrderDto?)null);

        // Act
        var result = await _controller.GetOrderById(orderId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    // ---------------- CREATE ORDER ----------------
    [Fact]
    public async Task CreateOrder_ShouldReturnOk_WhenSuccessful()
    {
        // Arrange
        var dto = new CreateOrderDto
        {
            CustomerID = "ALFKI",
            EmployeeID = 1,
            ShipCity = "Berlin"
        };

        _serviceMock
            .Setup(s => s.CreateAsync(dto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateOrder(dto);

        // Assert
        var okResult = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturn500_WhenExceptionOccurs()
    {
        // Arrange
        var dto = new CreateOrderDto
        {
            CustomerID = "ALFKI",
            EmployeeID = 1
        };

        _serviceMock
            .Setup(s => s.CreateAsync(dto))
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await _controller.CreateOrder(dto);

        // Assert
        var objResult = result.Should()
            .BeOfType<ObjectResult>()
            .Subject;

        objResult.StatusCode.Should().Be(500);
        objResult.Value.Should().Be("DB error");
    }

    // ---------------- UPDATE ORDER ----------------
    [Fact]
    public async Task UpdateOrder_ShouldReturnNoContent_WhenOrderExists()
    {
        // Arrange
        int orderId = 1;

        var dto = new UpdateOrderDto
        {
            CustomerID = "ALFKI",
            EmployeeID = 1,
            ShipCity = "Berlin"
        };

        _serviceMock
            .Setup(s => s.GetByIdAsync(orderId))
            .ReturnsAsync(new OrderDto { OrderID = orderId });

        _serviceMock
            .Setup(s => s.UpdateAsync(orderId, dto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateOrder(orderId, dto);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateOrder_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        int orderId = 999;

        var dto = new UpdateOrderDto
        {
            CustomerID = "ALFKI"
        };

        _serviceMock
            .Setup(s => s.GetByIdAsync(orderId))
            .ReturnsAsync((OrderDto?)null);

        // Act
        var result = await _controller.UpdateOrder(orderId, dto);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}