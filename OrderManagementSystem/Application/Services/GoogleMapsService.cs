using Newtonsoft.Json.Linq;
using OrderManagementSystem.Application.DTOs.AdressValidation;
using OrderManagementSystem.Application.Interfaces;

public class GoogleMapsService : IGoogleMapsService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "API_KEY"; // Replace with your actual API key

    public GoogleMapsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public virtual async Task<AddressValidationResponse?> ValidateAddress(string address)
    {
        var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse(content);

        var result = json["results"]?.FirstOrDefault();
        if (result == null)
            return null;

        var components = result["address_components"];

        string streetNumber = GetComponent(components, "street_number");
        string route = GetComponent(components, "route");

        return new AddressValidationResponse
        {
            FormattedAddress = result["formatted_address"]?.ToString(),
            Latitude = (double)result["geometry"]["location"]["lat"],
            Longitude = (double)result["geometry"]["location"]["lng"],

            Street = $"{streetNumber} {route}".Trim(),
            City = GetComponent(components, "locality")
                ?? GetComponent(components, "postal_town"),
            State = GetComponent(components, "administrative_area_level_1"),
            Postal = GetComponent(components, "postal_code"),
            Country = GetComponent(components, "country")
        };
    }

    private string GetComponent(JToken components, string type)
    {
        var comp = components.FirstOrDefault(c =>
            c["types"] != null &&
            c["types"].Any(t => t.ToString() == type)
        );

        return comp?["long_name"]?.ToString();
    }
}