using Microsoft.AspNetCore.Components.WebView.Maui;
using PPPOCMobileBlazorMAUI.Data;
using PPPOCMobileBlazorMAUI.Interface;
using PPPOCMobileBlazorMAUI.Services;
using PPPOCMobileBlazorMAUI.SQLiteRepository;

namespace PPPOCMobileBlazorMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSingleton(new AccountRepository("accounts.db"));
            builder.Services.AddSingleton<IDialogService, DialogService>();

            return builder.Build();
        }
    }
}