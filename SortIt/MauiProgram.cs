using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SortIt.Services;
using SortIt.ViewModels;
using SortIt.Views;
using Syncfusion.Licensing;
using Syncfusion.Maui.Core.Hosting;

namespace SortIt
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // Loeb Syncfusion license key appsettings.json failist
            using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").Result;
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            string licenseKey = config["Syncfusion:LicenseKey"];
            SyncfusionLicenseProvider.RegisterLicense(licenseKey);

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("TitilliumWeb-Regular.ttf", "Titillium");
                });

            builder.Services.AddSingleton<AudioService>();
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddHttpClient<CloudVisionAPIService>();

            builder.Services.AddTransient<WasteDetectionViewModel>();
            builder.Services.AddTransient<WasteDetectionPage>();
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "sortit.db3");

            builder.Services.AddSingleton(new DatabaseService(dbPath));

            builder.Services.AddTransient<StatisticsViewModel>();
            builder.Services.AddTransient<StatisticsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
