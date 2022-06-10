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
}