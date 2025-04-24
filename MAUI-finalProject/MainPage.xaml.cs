 using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using MAUI_finalProject.Services;

namespace MAUI_finalProject
{
    public partial class MainPage : ContentPage
    {
        // Current zoom level (approximate)
        private double _currentZoomKilometers = 5.0;

        // Default location is the location near my apartment for Levittown PA.
        private readonly Location _defaultLocation = new(40.170020, -74.833359);

        private readonly LocationService _locationService;
        private readonly DBConnection _locationDatabase;

        public MainPage(LocationService locationService, DBConnection locationDatabase)
        {
            InitializeComponent();
            _locationService = locationService;
            _locationDatabase = locationDatabase;

            InitializeMap();
            _ = ShowUserLocationAsync();
            StartLocationTimer();
            _ = PrintSavedLocationsAsync();
        }

        private void InitializeMap()
        {
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                _defaultLocation,
                Distance.FromKilometers(_currentZoomKilometers)));

            MyMap.Pins.Add(new Pin
            {
                Label = "Default Location",
                Location = _defaultLocation,
                Type = PinType.Generic
            });
        }

        //This method records the location every 30 minutes.
        private void StartLocationTimer()
        {
            this.Dispatcher.StartTimer(TimeSpan.FromMinutes(30), () =>
            {
                _ = MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    var location = await _locationService.GetCurrentLocationAsync();
                    if (location != null)
                    {
                        Console.WriteLine($"Location Saved: Lat={location.Latitude}, Lng={location.Longitude}, Time={DateTime.UtcNow}");
                        await _locationDatabase.SaveLocationAsync(location);
                    }
                    else
                    {
                        Console.WriteLine("Location could not be tracked. There is some error here.");
                    }
                });

                return true; // Repeat the timer
            });

        }


        //Adds locations pins on the existing map.
        private async Task PrintSavedLocationsAsync()
        {
            var locations = await _locationDatabase.GetLocationsAsync();

            foreach (var loc in locations)
            {
                //add logic to print dots here
                var redPin = new Pin
                {
                    Label = $"Saved @ {loc.Timestamp.ToShortTimeString()}",
                    Location = new Location(loc.Latitude, loc.Longitude),
                    Type = PinType.Generic
                };


                MyMap.Pins.Add(redPin);
                Console.WriteLine($"Saved Location: Lat={loc.Latitude}, Lng={loc.Longitude}, Time={loc.Timestamp}");
            }
        }

        //This method allows adjusting the zoom level of the map.
        private void OnZoomInClicked(object sender, EventArgs e)
        {
            _currentZoomKilometers *= 0.75;
            _currentZoomKilometers = Math.Max(_currentZoomKilometers, 0.5);
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                GetMapCenter(),
                Distance.FromKilometers(_currentZoomKilometers)));
        }

        //This method allows adjusting the zoom level of the map.
        private void OnZoomOutClicked(object sender, EventArgs e)
        {
            _currentZoomKilometers *= 1.5;
            _currentZoomKilometers = Math.Min(_currentZoomKilometers, 500);
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                GetMapCenter(),
                Distance.FromKilometers(_currentZoomKilometers)));
        }

        
        private Location GetMapCenter()
        {
            return MyMap.VisibleRegion?.Center ?? _defaultLocation;
        }

        //This takes to the area where heatmap is supposed to be shown.
        private async Task ShowUserLocationAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission Denied", "Location permission is required to show your position.", "OK");
                return;
            }

            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location != null)
                {
                    var userPosition = new Location(location.Latitude, location.Longitude);
                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(userPosition, Distance.FromKilometers(1)));

                    var userPin = new Pin
                    {
                        Label = "This is the current location",
                        Location = userPosition,
                        Type = PinType.Place
                    };

                    //This is used for debugging purpose only. 
                    //MyMap.Pins.Add(userPin);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unable to get location: {ex.Message}", "OK");
            }
        }
    }
}
