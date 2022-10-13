using ZXing.Net.Maui;

namespace RoyalMailPOC;

public partial class Barcode : ContentPage
{
    public Barcode()
    {
        InitializeComponent();

        barcodeView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormat.Code39,
            AutoRotate = false,
            Multiple = false,
            TryHarder = true
            
        };
    }

    protected async void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        try
        {
            foreach (var barcode in e.Results)
                Console.WriteLine($"Barcodes: {barcode.Format} -> {barcode.Value}");


            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                var r = e.Results.First();

                barcodeGenerator.Value = r.Value;
                barcodeGenerator.Format = r.Format;

                DisplayAlert("Barcode detected", $"Value: {r.Value}, BarcodeFormat: {r.Format}", "Ok");
            });
        }
        catch (Exception)
        {

            throw;
        }
    }

    void SwitchCameraButton_Clicked(object sender, EventArgs e)
    {
        barcodeView.CameraLocation = barcodeView.CameraLocation == CameraLocation.Rear ? CameraLocation.Front : CameraLocation.Rear;
    }

    void TorchButton_Clicked(object sender, EventArgs e)
    {
        barcodeView.IsTorchOn = !barcodeView.IsTorchOn;
    }
}
