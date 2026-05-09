using FluentAssertions;
using Moq;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Application.Services;
using OrderManagementSystem.Models;

namespace Backend.UnitTests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repoMock = new Mock<IProductRepository>();
        _service = new ProductService(_repoMock.Object);
    }

    // ---------------- GET ALL ----------------
    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedProducts()
    {
        // Arrange
        var fakeList = new List<Product>
        {
            new Product
            {
                ProductID = 1,
                ProductName = "Keyboard",
                UnitPrice = 50m
            },
            new Product
            {
                ProductID = 2,
                ProductName = "Mouse",
                UnitPrice = 25m
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
        result[0].ProductName.Should().Be("Keyboard");
        result[0].UnitPrice.Should().Be(50m);

        result[1].ProductID.Should().Be(2);
        result[1].ProductName.Should().Be("Mouse");
        result[1].UnitPrice.Should().Be(25m);
    }

    // ---------------- GET BY ID ----------------
    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenExists()
    {
        // Arrange
        int id = 1;

        var fakeProduct = new Product
        {
            ProductID = id,
            ProductName = "Laptop",
            UnitPrice = 1000m
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(fakeProduct);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.ProductID.Should().Be(id);
        result.ProductName.Should().Be("Laptop");
        result.UnitPrice.Should().Be(1000m);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }
}