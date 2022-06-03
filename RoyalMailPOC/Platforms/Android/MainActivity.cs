using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.NFC;

namespace RoyalMailPOC;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        // Plugin NFC: Initialization
        CrossNFC.Init(this);
    }

    protected override void OnResume()
    {
        base.OnResume();
        // Plugin NFC: Restart NFC listening on resume (needed for Android 10+) 
        CrossNFC.OnResume();
    }

    protected override void OnNewIntent(Intent? intent)
    {
        base.OnNewIntent(intent);
        // Plugin NFC: Tag Discovery Interception
        CrossNFC.OnNewIntent(intent);
    }
}
