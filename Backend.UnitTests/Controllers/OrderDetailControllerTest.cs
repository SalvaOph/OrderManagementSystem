using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementSystem.Application.Dtos.OrderDetail;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Controllers;

public class OrderDetailControllerTests
{
    private readonly Mock<IOrderDetailService> _serviceMock;
    private readonly OrderDetailController _controller;

    public OrderDetailControllerTests()
    {
        _serviceMock = new Mock<IOrderDetailService>();
        _controller = new OrderDetailController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithList()
    {
        // Arrange
        var list = new List<OrderDetailDto>
        {
            new OrderDetailDto
            {
                ProductID = 1,
                UnitPrice = 10,
                Quantity = 5,
                Discount = 0
            }
        };

        _serviceMock
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(list);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedData = Assert.IsAssignableFrom<IEnumerable<OrderDetailDto>>(okResult.Value);

        Assert.Single(returnedData);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        // Arrange
        var dto = new OrderDetailDto
        {
            ProductID = 1,
            UnitPrice = 20,
            Quantity = 3,
            Discount = 0
        };

        _serviceMock
            .Setup(s => s.GetByIdAsync(1, 1))
            .ReturnsAsync(dto);

        // Act
        var result = await _controller.GetById(1, 1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsType<OrderDetailDto>(okResult.Value);

        Assert.Equal(1, returnedDto.ProductID);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.GetByIdAsync(1, 1))
            .ReturnsAsync((OrderDetailDto)null);

        // Act
        var result = await _controller.GetById(1, 1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsOk()
    {
        // Arrange
        var dto = new CreateOrderDetailDto
        {
            OrderID = 1,
            ProductID = 1,
            UnitPrice = 15,
            Quantity = 2,
            Discount = 0
        };

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsType<CreateOrderDetailDto>(okResult.Value);

        Assert.Equal(dto.ProductID, returnedDto.ProductID);

        _serviceMock.Verify(
            s => s.CreateAsync(dto),
            Times.Once
        );
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenFound()
    {
        // Arrange
        var existing = new OrderDetailDto
        {
            ProductID = 1,
            UnitPrice = 10,
            Quantity = 1,
            Discount = 0
        };

        var updateDto = new UpdateOrderDetailDto
        {
            ProductID = 1,
            UnitPrice = 20,
            Quantity = 5,
            Discount = 0
        };

        _serviceMock
            .Setup(s => s.GetByIdAsync(1, 1))
            .ReturnsAsync(existing);

        // Act
        var result = await _controller.Update(1, 1, updateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);

        _serviceMock.Verify(
            s => s.UpdateAsync(1, 1, updateDto),
            Times.Once
        );
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenMissing()
    {
        // Arrange
        var updateDto = new UpdateOrderDetailDto
        {
            ProductID = 1,
            UnitPrice = 20,
            Quantity = 5,
            Discount = 0
        };

        _serviceMock
            .Setup(s => s.GetByIdAsync(1, 1))
            .ReturnsAsync((OrderDetailDto)null);

        // Act
        var result = await _controller.Update(1, 1, updateDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}