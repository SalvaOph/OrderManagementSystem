using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementSystem.Application.DTOs.AdressValidation;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Controllers;

public class AddressControllerTests
{
    private readonly Mock<IGoogleMapsService> _serviceMock;
    private readonly AddressController _controller;

    public AddressControllerTests()
    {
        _serviceMock = new Mock<IGoogleMapsService>();
        _controller = new AddressController(_serviceMock.Object);
    }

    [Fact]
    public async Task Validate_ReturnsOk_WhenAddressIsValid()
    {
        // Arrange
        var request = new AddressValidationRequest
        {
            Address = "1600 Amphitheatre Parkway"
        };

        var response = new AddressValidationResponse
        {
            FormattedAddress = "1600 Amphitheatre Parkway",
            Latitude = 37.422,
            Longitude = -122.084,
            Street = "Amphitheatre Parkway",
            City = "Mountain View",
            State = "California",
            Postal = "94043",
            Country = "USA"
        };

        _serviceMock
            .Setup(s => s.ValidateAddress(request.Address))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Validate(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        var returnedValue =
            Assert.IsType<AddressValidationResponse>(okResult.Value);

        Assert.Equal(response.City, returnedValue.City);
    }

    [Fact]
    public async Task Validate_ReturnsBadRequest_WhenAddressIsInvalid()
    {
        // Arrange
        var request = new AddressValidationRequest
        {
            Address = "Invalid"
        };

        _serviceMock
            .Setup(s => s.ValidateAddress(request.Address))
            .ReturnsAsync((AddressValidationResponse)null);

        // Act
        var result = await _controller.Validate(request);

        // Assert
        var badRequest =
            Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal("Invalid address", badRequest.Value);
    }
}