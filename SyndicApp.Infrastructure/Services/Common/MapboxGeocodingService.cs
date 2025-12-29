using Microsoft.Extensions.Configuration;
using SyndicApp.Application.Interfaces.Common;
using System.Net.Http.Json;

public class MapboxGeocodingService : IGeocodingService
{
    private readonly HttpClient _http;
    private readonly string _token;

    public MapboxGeocodingService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _token = config["Mapbox:AccessToken"]
            ?? throw new InvalidOperationException("Mapbox token manquant");
    }

    public async Task<(double lat, double lng)?> GeocodeAsync(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return null;

        var url =
            $"https://api.mapbox.com/geocoding/v5/mapbox.places/{Uri.EscapeDataString(address)}.json" +
            $"?limit=1&access_token={_token}";

        var response = await _http.GetFromJsonAsync<MapboxResponse>(url);

        var feature = response?.features?.FirstOrDefault();
        if (feature == null) return null;

        return (feature.center[1], feature.center[0]); // lat, lng
    }

    private class MapboxResponse
    {
        public List<MapboxFeature>? features { get; set; }
    }

    private class MapboxFeature
    {
        public double[] center { get; set; } = Array.Empty<double>();
    }
}
