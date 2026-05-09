using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementSystem.Application.Dtos.Product;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Controllers;

namespace Backend.UnitTests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _serviceMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _serviceMock = new Mock<IProductService>();

        _controller = new ProductsController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnOk_WithProducts()
    {
        // Arrange
        var fakeProducts = new List<ProductDto>
        {
            new ProductDto
            {
                ProductID = 1,
                ProductName = "Laptop",
                UnitPrice = 1200.00m
            },
            new ProductDto
            {
                ProductID = 2,
                ProductName = "Mouse",
                UnitPrice = 25.50m
            }
        };

        _serviceMock
            .Setup(service => service.GetAllAsync())
            .ReturnsAsync(fakeProducts);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value.Should().BeEquivalentTo(fakeProducts);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnOk_WhenProductExists()
    {
        // Arrange
        int productId = 1;

        var fakeProduct = new ProductDto
        {
            ProductID = productId,
            ProductName = "Laptop",
            UnitPrice = 1200.00m
        };

        _serviceMock
            .Setup(service => service.GetByIdAsync(productId))
            .ReturnsAsync(fakeProduct);

        // Act
        var result = await _controller.GetProductById(productId);

        // Assert
        var okResult = result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;

        okResult.Value.Should().BeEquivalentTo(fakeProduct);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        int productId = 999;

        _serviceMock
            .Setup(service => service.GetByIdAsync(productId))
            .ReturnsAsync((ProductDto?)null);

        // Act
        var result = await _controller.GetProductById(productId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}