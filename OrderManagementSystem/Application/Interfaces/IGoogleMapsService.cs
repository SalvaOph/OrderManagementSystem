using Newtonsoft.Json.Linq;
using OrderManagementSystem.Application.DTOs.AdressValidation;

namespace OrderManagementSystem.Application.Interfaces;

public interface IGoogleMapsService
{
    Task<AddressValidationResponse?> ValidateAddress(string address);
}