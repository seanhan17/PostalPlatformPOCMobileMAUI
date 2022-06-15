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
            PermissionStatus cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
            PermissionStatus storageWrite = await Permissions.RequestAsync<Permissions.StorageWrite>();

            if (cameraStatus != PermissionStatus.Granted)
            {
                await DisplayAlert("Camera Permission", "Camera permission has been disabled. Please enable it to use the camera", "Ok");
                return;
            }

            if (storageWrite != PermissionStatus.Granted)
            {
                await DisplayAlert("Media Permission", "Media permission has been disabled. Please enable it to use the media", "Ok");
                return;
            }

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