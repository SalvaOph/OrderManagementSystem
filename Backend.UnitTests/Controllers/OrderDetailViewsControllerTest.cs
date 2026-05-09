using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementSystem.Application.DTOs.OrderDetailViews;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Controllers;

namespace Backend.UnitTests.Controllers;

public class OrderDetailViewsControllerTests
{
    private readonly Mock<IOrderDetailViewService> _serviceMock;
    private readonly OrderDetailViewsController _controller;

    public OrderDetailViewsControllerTests()
    {
        _serviceMock = new Mock<IOrderDetailViewService>();

        _controller = new OrderDetailViewsController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithResults()
    {
        // Arrange
        var fakeResults = new List<OrderDetailViewDto>
        {
            new OrderDetailViewDto
            {
                OrderID = 1,
                CustomerName = "John Doe",
                Freight = 25.50m,
                TotalAmount = 150.00m
            },
            new OrderDetailViewDto
            {
                OrderID = 2,
                CustomerName = "Jane Smith",
                Freight = 10.00m,
                TotalAmount = 75.00m
            }
        };

        _serviceMock
            .Setup(service => service.GetAllAsync())
            .ReturnsAsync(fakeResults);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value.Should().BeEquivalentTo(fakeResults);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenOrderExists()
    {
        // Arrange
        int orderId = 1;

        var fakeOrder = new OrderDetailViewDto
        {
            OrderID = orderId,
            CustomerName = "John Doe",
            Freight = 20.00m,
            TotalAmount = 100.00m
        };

        _serviceMock
            .Setup(service => service.GetByIdAsync(orderId))
            .ReturnsAsync(fakeOrder);

        // Act
        var result = await _controller.GetById(orderId);

        // Assert
        var okResult = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value.Should().BeEquivalentTo(fakeOrder);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        int orderId = 999;

        _serviceMock
            .Setup(service => service.GetByIdAsync(orderId))
            .ReturnsAsync((OrderDetailViewDto?)null);

        // Act
        var result = await _controller.GetById(orderId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}