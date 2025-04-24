using MAUI_finalProject.Models;

namespace MAUI_finalProject.Services
{
    public class LocationService
    {
        public async Task<LocationInfo?> GetCurrentLocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(10)
                    });

                if (location != null)
                {
                    return new LocationInfo
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Timestamp = DateTime.UtcNow
                    };
                }
            }
            catch (Exception ex)
            {
                const string errorMessage = "Unable to get location. Please check your settings.";
                Console.WriteLine($"{errorMessage} {ex.Message}");
            }

            return null;
        }
    }
}
