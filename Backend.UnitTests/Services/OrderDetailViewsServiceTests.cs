using FluentAssertions;
using Moq;
using OrderManagementSystem.Application.DTOs.OrderDetailViews;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Application.Services;
using Xunit;

namespace Backend.UnitTests.Services;

public class OrderDetailViewServiceTests
{
    private readonly Mock<IOrderDetailViewRepository> _repoMock;
    private readonly OrderDetailViewService _service;

    public OrderDetailViewServiceTests()
    {
        _repoMock = new Mock<IOrderDetailViewRepository>();

        _service = new OrderDetailViewService(_repoMock.Object);
    }

    // ---------------- GET ALL ----------------
    [Fact]
    public async Task GetAllAsync_ShouldReturnListFromRepository()
    {
        // Arrange
        var fakeList = new List<OrderDetailViewDto>
        {
            new OrderDetailViewDto
            {
                OrderID = 1,
                CustomerName = "John Doe",
                TotalAmount = 100m
            },
            new OrderDetailViewDto
            {
                OrderID = 2,
                CustomerName = "Jane Doe",
                TotalAmount = 200m
            }
        };

        _repoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(fakeList);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].OrderID.Should().Be(1);
        result[1].OrderID.Should().Be(2);
    }

    // ---------------- GET BY ID ----------------
    [Fact]
    public async Task GetByIdAsync_ShouldReturnItem_WhenExists()
    {
        // Arrange
        int orderId = 1;

        var fakeDto = new OrderDetailViewDto
        {
            OrderID = orderId,
            CustomerName = "John Doe",
            TotalAmount = 150m
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync(fakeDto);

        // Act
        var result = await _service.GetByIdAsync(orderId);

        // Assert
        result.Should().NotBeNull();
        result!.OrderID.Should().Be(orderId);
        result.CustomerName.Should().Be("John Doe");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((OrderDetailViewDto?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }
}