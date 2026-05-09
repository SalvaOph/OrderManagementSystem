using FluentAssertions;
using Moq;
using OrderManagementSystem.Application.Dtos.OrderDetail;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Application.Services;
using OrderManagementSystem.Models;

namespace Backend.UnitTests.Services;

public class OrderDetailServiceTests
{
    private readonly Mock<IOrderDetailRepository> _repoMock;
    private readonly Mock<IOrderRepository> _orderRepoMock;
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly OrderDetailService _service;

    public OrderDetailServiceTests()
    {
        _repoMock = new Mock<IOrderDetailRepository>();
        _orderRepoMock = new Mock<IOrderRepository>();
        _productRepoMock = new Mock<IProductRepository>();

        _service = new OrderDetailService(
            _repoMock.Object,
            _orderRepoMock.Object,
            _productRepoMock.Object
        );
    }

    // ---------------- GET ALL ----------------
    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedOrderDetails()
    {
        // Arrange
        var fakeList = new List<OrderDetail>
        {
            new OrderDetail
            {
                ProductID = 1,
                UnitPrice = 10m,
                Quantity = 2,
                Discount = 0f
            },
            new OrderDetail
            {
                ProductID = 2,
                UnitPrice = 20m,
                Quantity = 1,
                Discount = 0.1f
            }
        };

        _repoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(fakeList);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result[0].ProductID.Should().Be(1);
        result[0].UnitPrice.Should().Be(10m);
        result[0].Quantity.Should().Be(2);
        result[0].Discount.Should().Be(0f);
    }

    // ---------------- GET BY ID ----------------
    [Fact]
    public async Task GetByIdAsync_ShouldReturnOrderDetail_WhenExists()
    {
        // Arrange
        int orderId = 1;
        int productId = 2;

        var fakeEntity = new OrderDetail
        {
            ProductID = productId,
            UnitPrice = 15m,
            Quantity = 3,
            Discount = 0f
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(orderId, productId))
            .ReturnsAsync(fakeEntity);

        // Act
        var result = await _service.GetByIdAsync(orderId, productId);

        // Assert
        result.Should().NotBeNull();
        result!.ProductID.Should().Be(productId);
        result.UnitPrice.Should().Be(15m);
        result.Quantity.Should().Be(3);
        result.Discount.Should().Be(0f);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((OrderDetail?)null);

        // Act
        var result = await _service.GetByIdAsync(1, 1);

        // Assert
        result.Should().BeNull();
    }

    // ---------------- CREATE ----------------
    [Fact]
    public async Task CreateAsync_ShouldCreateOrderDetail_WhenValid()
    {
        // Arrange
        var dto = new CreateOrderDetailDto
        {
            OrderID = 1,
            ProductID = 2,
            UnitPrice = 10m,
            Quantity = 1,
            Discount = 0f
        };

        _orderRepoMock
            .Setup(r => r.GetByIdAsync(dto.OrderID))
            .ReturnsAsync(new Order());

        _productRepoMock
            .Setup(r => r.GetByIdAsync(dto.ProductID))
            .ReturnsAsync(new Product());

        _repoMock
            .Setup(r => r.GetByIdAsync(dto.OrderID, dto.ProductID))
            .ReturnsAsync((OrderDetail?)null);

        _repoMock
            .Setup(r => r.CreateAsync(It.IsAny<OrderDetail>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.CreateAsync(dto);

        // Assert
        _repoMock.Verify(r =>
            r.CreateAsync(It.IsAny<OrderDetail>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenOrderNotFound()
    {
        // Arrange
        var dto = new CreateOrderDetailDto
        {
            OrderID = 1,
            ProductID = 2
        };

        _orderRepoMock
            .Setup(r => r.GetByIdAsync(dto.OrderID))
            .ReturnsAsync((Order?)null);

        // Act
        Func<Task> act = async () => await _service.CreateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Order not found");
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenProductNotFound()
    {
        // Arrange
        var dto = new CreateOrderDetailDto
        {
            OrderID = 1,
            ProductID = 2
        };

        _orderRepoMock
            .Setup(r => r.GetByIdAsync(dto.OrderID))
            .ReturnsAsync(new Order());

        _productRepoMock
            .Setup(r => r.GetByIdAsync(dto.ProductID))
            .ReturnsAsync((Product?)null);

        // Act
        Func<Task> act = async () => await _service.CreateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Product not found");
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenOrderDetailAlreadyExists()
    {
        // Arrange
        var dto = new CreateOrderDetailDto
        {
            OrderID = 1,
            ProductID = 2
        };

        _orderRepoMock
            .Setup(r => r.GetByIdAsync(dto.OrderID))
            .ReturnsAsync(new Order());

        _productRepoMock
            .Setup(r => r.GetByIdAsync(dto.ProductID))
            .ReturnsAsync(new Product());

        _repoMock
            .Setup(r => r.GetByIdAsync(dto.OrderID, dto.ProductID))
            .ReturnsAsync(new OrderDetail());

        // Act
        Func<Task> act = async () => await _service.CreateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("OrderDetail already exists");
    }

    // ---------------- UPDATE ----------------
    [Fact]
    public async Task UpdateAsync_ShouldUpdate_WhenExists()
    {
        // Arrange
        int orderId = 1;
        int productId = 2;

        var existing = new OrderDetail
        {
            ProductID = productId,
            UnitPrice = 10m,
            Quantity = 1,
            Discount = 0f
        };

        var dto = new UpdateOrderDetailDto
        {
            UnitPrice = 20m,
            Quantity = 5,
            Discount = 0.1f
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(orderId, productId))
            .ReturnsAsync(existing);

        _repoMock
            .Setup(r => r.UpdateAsync(existing))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(orderId, productId, dto);

        // Assert
        existing.UnitPrice.Should().Be(20m);
        existing.Quantity.Should().Be(5);
        existing.Discount.Should().Be(0.1f);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenNotFound()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((OrderDetail?)null);

        var dto = new UpdateOrderDetailDto();

        // Act
        Func<Task> act = async () => await _service.UpdateAsync(1, 1, dto);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("OrderDetail not found");
    }
}