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
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<IMap>(Map.Default);
        builder.Services.AddSingleton(new AccountRepository("accounts.db"));
        builder.Services.AddScoped<SQLite>();

        return builder.Build();
	}
}
