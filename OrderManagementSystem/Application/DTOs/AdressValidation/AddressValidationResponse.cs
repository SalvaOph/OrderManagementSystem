namespace OrderManagementSystem.Application.DTOs.AdressValidation;
public class AddressValidationResponse
{
    public string FormattedAddress { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Postal { get; set; }
    public string Country { get; set; }
}