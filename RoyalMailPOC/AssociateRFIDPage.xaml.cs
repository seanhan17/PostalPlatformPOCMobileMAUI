using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.Configuration;
using Plugin.Maui.Audio;
using ZXing;
using ZXing.Net.Maui;
using BarcodeFormat = ZXing.Net.Maui.BarcodeFormat;

namespace RoyalMailPOC;

public partial class AssociateRFIDPage : ContentPage
{
	private ScanField RFIDId;
	private ScanField ItemId;
    public ObservableCollection<History> Histories { get; set; }

    private WebAPILayer wapi = new WebAPILayer();
    private bool isSuccess;
    private string resultMessage ="";

    public AssociateRFIDPage()
	{
		InitializeComponent();

        barcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.OneDimensional,
            AutoRotate = false,
            Multiple = false,
            TryHarder = true,

        };

        ResetScan();


        Histories = new ObservableCollection<History>();
    }

    private void CameraBarcodeReaderView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
	{
        #region Scanning

        MainThread.BeginInvokeOnMainThread(async () =>
		{
            if (barcodeReader.IsDetecting)
            {
                barcodeReader.IsDetecting = false;

                var beep = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("soundBarcodeBeep.mp3"));

                beep.Loop = false;
                beep.Play();

                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200));

                if (RFIDId.IsScanning)
				{
                    barcodeBorder.Stroke = Color.FromArgb("#27ae60");
                    RFIDId.Value = e.Results[0].Value;
                    lblRFIDIdField.Text = RFIDId.Value;

                    RFIDId.IsScanning = false;
                    ItemId.IsScanning = true;

                    lblItemIdField.Text = "Now scanning...";
                    await Task.Delay(2000);
                    barcodeReader.IsDetecting = true;
                    barcodeBorder.Stroke = Color.FromArgb("#e74c3c");
                }
                else if (ItemId.IsScanning)
				{
                    barcodeBorder.Stroke = Color.FromArgb("#27ae60");
                    ItemId.Value = e.Results[0].Value;
                    lblItemIdField.Text = ItemId.Value;

                    ItemId.IsScanning = false;
                    activityIndicator.IsVisible = true;
                    activityIndicator.IsRunning = true;
                    mainLayout.IsEnabled = false;
                    await Task.Delay(2000);
                    if (ValidateScan())
                    {

                        var response = await wapi.AssociateRFID(1099, ItemId.Value, RFIDId.Value);

                        if(response != null)
                        {
                            var resultCode = Convert.ToInt32(response.ResultCode.ToString());
                            if (resultCode == 0)
                            {
                                isSuccess = true;
                            }
                            else
                            {
                                resultMessage = response.ResultMessage;
                            }
                        }

                        var snackbarColor = Color.FromArgb("#27ae60");
                        var snackbarMsg = $"Item ID {lblItemIdField.Text} successfully associated to RFID {lblRFIDIdField.Text}.";
                        if (!isSuccess)
                        {
                            snackbarColor = Color.FromArgb("#e74c3c");
                            snackbarMsg = resultMessage;
                        }
                        var options = new SnackbarOptions
                        {
                            BackgroundColor = snackbarColor,
                            TextColor = Colors.White,
                            ActionButtonTextColor = Colors.White,
                            CornerRadius = new CornerRadius(10),
                            Font = Microsoft.Maui.Font.SystemFontOfSize(12),
                        };
                        var snackbar = Snackbar.Make(snackbarMsg,null, "OK", TimeSpan.FromSeconds(3),options, anchor:snackbarAnchor);
                        
                        await snackbar.Show();

                        Histories.Insert(0,new History
                        {
                            RFIDId = RFIDId.Value,
                            ItemId = ItemId.Value,
                            Status = isSuccess ? "Successful" : "Failed",
                            StatusColor = isSuccess ? Color.FromArgb("#27ae60") : Color.FromArgb("#e74c3c")
                        });

                        HistoryListView.ItemsSource = Histories;

                        ResetScan();
                        activityIndicator.IsVisible = false;
                        activityIndicator.IsRunning = false;
                        mainLayout.IsEnabled = true;
                    }
                }

            }
		});
		#endregion

    }

    private bool ValidateScan()
    {
        if (!RFIDId.IsScanning && !ItemId.IsScanning && !string.IsNullOrEmpty(RFIDId.Value) && !string.IsNullOrEmpty(ItemId.Value))
        {
			return true;
        }
		else
		{
			return false;
		}
    }

    private void ResetScan()
    {
        lblRFIDIdField.Text = "Now scanning...";
        lblItemIdField.Text = "-";
        RFIDId = new ScanField();
        ItemId = new ScanField();

        RFIDId.IsScanning = true;
        barcodeReader.IsDetecting = true;
        barcodeBorder.Stroke = Color.FromArgb("#e74c3c");
    }

    private async void OnEnterManuallyClicked(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new AssociateRFIDManualPage());
    }

    protected override async void OnAppearing()
    {
        RFIDId.IsScanning = true;
        barcodeReader.IsDetecting = true;
    }

    protected override async void OnDisappearing()
    {
        barcodeReader.IsDetecting = false;
        RFIDId.IsScanning = false;
        ItemId.IsScanning = false;
    }

    public class History
    {
        public string RFIDId { get; set; }
        public string ItemId { get; set; }
        public string Status { get; set; }
        public Color StatusColor { get; set; }
    }
}
