using FluentAssertions;
using Moq;
using OrderManagementSystem.Application.Dtos.Order;
using OrderManagementSystem.Application.Dtos.OrderDetail;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Application.Services;
using OrderManagementSystem.Models;
using Xunit;

namespace Backend.UnitTests.Services;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _repoMock;
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _repoMock = new Mock<IOrderRepository>();
        _service = new OrderService(_repoMock.Object);
    }

    // ---------------- GET ALL ----------------
    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedOrders()
    {
        // Arrange
        var fakeList = new List<Order>
        {
            new Order
            {
                OrderID = 1,
                CustomerID = "C1",
                EmployeeID = 10,
                ShipVia = 2,
                OrderDate = DateTime.UtcNow,
                ShipAddress = "Street 1",
                ShipCity = "City A"
            },
            new Order
            {
                OrderID = 2,
                CustomerID = "C2",
                EmployeeID = 20,
                ShipVia = 1,
                OrderDate = DateTime.UtcNow,
                ShipAddress = "Street 2",
                ShipCity = "City B"
            }
        };

        _repoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(fakeList);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result[0].OrderID.Should().Be(1);
        result[0].CustomerID.Should().Be("C1");
        result[1].OrderID.Should().Be(2);
    }

    // ---------------- GET BY ID ----------------
    [Fact]
    public async Task GetByIdAsync_ShouldReturnOrder_WhenExists()
    {
        // Arrange
        var orderId = 1;

        var fakeOrder = new Order
        {
            OrderID = orderId,
            CustomerID = "C1",
            EmployeeID = 10,
            ShipVia = 2,
            OrderDate = DateTime.UtcNow,
            ShipAddress = "Street 1",
            ShipCity = "City A"
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync(fakeOrder);

        // Act
        var result = await _service.GetByIdAsync(orderId);

        // Assert
        result.Should().NotBeNull();
        result!.OrderID.Should().Be(orderId);
        result.CustomerID.Should().Be("C1");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Order?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }

    // ---------------- CREATE ----------------
    [Fact]
    public async Task CreateAsync_ShouldMapAndCallRepository()
    {
        // Arrange
        var dto = new CreateOrderDto
        {
            CustomerID = "C1",
            EmployeeID = 1,
            ShipVia = 2,
            ShipAddress = "Street 1",
            ShipCity = "City A",
            OrderDate = DateTime.UtcNow,
            OrderDetails = new List<CreateOrderDetailDto>
            {
                new CreateOrderDetailDto
                {
                    ProductID = 1,
                    UnitPrice = 10m,
                    Quantity = 2,
                    Discount = 0f
                }
            }
        };

        Order? capturedOrder = null;

        _repoMock
            .Setup(r => r.CreateAsync(It.IsAny<Order>()))
            .Callback<Order>(o => capturedOrder = o)
            .Returns(Task.CompletedTask);

        // Act
        await _service.CreateAsync(dto);

        // Assert
        _repoMock.Verify(r => r.CreateAsync(It.IsAny<Order>()), Times.Once);

        capturedOrder.Should().NotBeNull();
        capturedOrder!.CustomerID.Should().Be("C1");
        capturedOrder.OrderDetails.Should().HaveCount(1);
        capturedOrder.OrderDetails.First().ProductID.Should().Be(1);
    }

    // ---------------- UPDATE ----------------
    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingOrder()
    {
        // Arrange
        var orderId = 1;

        var existing = new Order
        {
            OrderID = orderId,
            CustomerID = "OLD",
            EmployeeID = 1,
            ShipVia = 1,
            OrderDate = DateTime.UtcNow,
            ShipAddress = "Old",
            ShipCity = "Old City",
            OrderDetails = new List<OrderDetail>()
        };

        var dto = new UpdateOrderDto
        {
            CustomerID = "NEW",
            EmployeeID = 2,
            ShipVia = 3,
            ShipAddress = "New Street",
            ShipCity = "New City",
            OrderDetails = new List<UpdateOrderDetailDto>
            {
                new UpdateOrderDetailDto
                {
                    ProductID = 1,
                    UnitPrice = 20m,
                    Quantity = 5,
                    Discount = 0.1f
                }
            }
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync(existing);

        _repoMock
            .Setup(r => r.UpdateAsync(existing))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(orderId, dto);

        // Assert
        existing.CustomerID.Should().Be("NEW");
        existing.ShipCity.Should().Be("New City");
        existing.OrderDetails.Should().HaveCount(1);

        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldDoNothing_WhenOrderNotFound()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Order?)null);

        var dto = new UpdateOrderDto();

        // Act
        await _service.UpdateAsync(1, dto);

        // Assert
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Never);
    }
}