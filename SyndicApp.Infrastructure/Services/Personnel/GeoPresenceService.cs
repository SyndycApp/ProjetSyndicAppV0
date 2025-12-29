using SyndicApp.Application.Interfaces.Personnel;

namespace SyndicApp.Infrastructure.Services.Personnel;

public class GeoPresenceService : IGeoPresenceService
{
    private const double EarthRadius = 6371000;

    public bool IsWithinRadius(
        double userLat,
        double userLng,
        double residenceLat,
        double residenceLng,
        double radiusMeters)
    {
        double dLat = ToRad(residenceLat - userLat);
        double dLon = ToRad(residenceLng - userLng);

        double a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(ToRad(userLat)) * Math.Cos(ToRad(residenceLat)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = EarthRadius * c;

        return distance <= radiusMeters;
    }

    private static double ToRad(double deg) => deg * Math.PI / 180;
}
