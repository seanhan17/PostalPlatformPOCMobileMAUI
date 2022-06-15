namespace RoyalMailPOC;

public partial class Mapzx : ContentPage
{
 //   IMap _map;
 //   public Mapzx(IMap map)
	//{
	//	InitializeComponent();
	//	_map = map;
	//}
    public Mapzx()
    {
        InitializeComponent();
    }
    public async void OnNavigate(object sender, EventArgs e)
    {
        var location = new Location(47.645160, -122.1306032);
        var options = new MapLaunchOptions { Name = "Microsoft Building 25" };

        try
        {
            await Map.Default.OpenAsync(location, options);
        }
        catch (Exception ex)
        {
            // No map application available to open
        }
    }

    public async void OnNavigateTwo(object sender, EventArgs e)
    {
        var placemark = new Placemark
        {
            CountryName = "United States",
            AdminArea = "WA",
            Thoroughfare = "Microsoft Building 25",
            Locality = "Redmond"
        };

        var options = new MapLaunchOptions { Name = "Microsoft Building 25" };

        try
        {
            await placemark.OpenMapsAsync(options);
        }
        catch (Exception ex)
        {
            // No map application available to open or placemark can not be located
        }
    }

    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;

    public async void OnGetGeolocation(object sender, EventArgs e)
    {
        try
        {
            PermissionStatus locationAlwaysStatus = await Permissions.RequestAsync<Permissions.LocationAlways>();
            PermissionStatus locationWhenInUseStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (!(locationAlwaysStatus == PermissionStatus.Granted || locationWhenInUseStatus == PermissionStatus.Granted))
            {
                await DisplayAlert("Geolocation Permission", "Geolocation permission has been disabled. Please enable it to use the Geolocation", "Ok");
                return;
            }

            _isCheckingLocation = true;

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            if (location != null)
                await DisplayAlert("Get Current Location", $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}", "Ok");
            else
                await DisplayAlert("Get Current Location", "Unable to retrieve current location", "Ok");
        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException
        catch (Exception ex)
        {
            // Unable to get location
        }
        finally
        {
            _isCheckingLocation = false;
        }
    }

    //public void CancelRequest()
    //{
    //    if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
    //        _cancelTokenSource.Cancel();
    //}
}