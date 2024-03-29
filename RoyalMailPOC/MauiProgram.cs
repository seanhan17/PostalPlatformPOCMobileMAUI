﻿using CommunityToolkit.Maui;
using Plugin.Maui.Audio;
using RoyalMailPOC.SQLiteRepository;
using ZXing.Net.Maui;

namespace RoyalMailPOC;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseBarcodeReader()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
                fonts.AddFont("Inter-Bold.ttf", "InterBold");
                fonts.AddFont("Inter-Medium.ttf", "InterMedium");
            })
        #region ZXing configuration
            .ConfigureMauiHandlers(h =>
            {
                h.AddHandler(typeof(ZXing.Net.Maui.Controls.CameraBarcodeReaderView), typeof(CameraBarcodeReaderViewHandler));
                h.AddHandler(typeof(ZXing.Net.Maui.Controls.CameraView), typeof(CameraViewHandler));
                h.AddHandler(typeof(ZXing.Net.Maui.Controls.BarcodeGeneratorView), typeof(BarcodeGeneratorViewHandler));
            });
        #endregion

        builder.Services.AddSingleton<IMap>(Map.Default);
        builder.Services.AddSingleton(new AccountRepository("accounts.db"));
        builder.Services.AddScoped<SQLite>();
        builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddTransient<AssociateRFIDPage>();

        return builder.Build();
	}
}