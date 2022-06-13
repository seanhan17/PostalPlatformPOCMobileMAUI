namespace RoyalMailPOC;

public partial class Camera : ContentPage
{
	public Camera()
	{
		InitializeComponent();
	}

    public async void OnTakePhoto(object sender, EventArgs e)
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            PermissionStatus status = await Permissions.RequestAsync<Permissions.Camera>();

            if (status == PermissionStatus.Granted)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    // save the file into local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    using Stream sourceStream = await photo.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                    await sourceStream.CopyToAsync(localFileStream);
                    _PhotoView.Source = localFilePath;
                }
            }
        }
    }
}