using ZXing.Net.Maui;

namespace RoyalMailPOC;

public partial class Barcode : ContentPage
{
	public Barcode()
	{
		InitializeComponent();

        barcodeView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = true
        };
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        foreach (var barcode in e.Results)
            Console.WriteLine($"Barcodes: {barcode.Format} -> {barcode.Value}");

        MainThread.InvokeOnMainThreadAsync(() =>
        {
            var r = e.Results.First();

            barcodeGenerator.Value = r.Value;
            barcodeGenerator.Format = r.Format;
        });
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