﻿using Android;
using Android.App;
using Android.Runtime;

//// Needed for Picking photo/video
//[assembly: UsesPermission(Android.Manifest.Permission.ReadExternalStorage)]

//// Needed for Taking photo/video
//[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]
//[assembly: UsesPermission(Android.Manifest.Permission.Camera)]

//// Add these properties if you would like to filter out devices that do not have cameras, or set to false to make them optional
//[assembly: UsesFeature("android.hardware.camera", Required = true)]
//[assembly: UsesFeature("android.hardware.camera.autofocus", Required = true)]

[assembly: UsesPermission(Android.Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessFineLocation)]
[assembly: UsesFeature("android.hardware.location", Required = false)]
[assembly: UsesFeature("android.hardware.location.gps", Required = false)]
[assembly: UsesFeature("android.hardware.location.network", Required = false)]
[assembly: UsesPermission(Manifest.Permission.AccessBackgroundLocation)]

namespace PPPOCMobileBlazorMAUI
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}