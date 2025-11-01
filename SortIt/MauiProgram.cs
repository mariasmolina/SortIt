using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SortIt.Services;
using SortIt.ViewModels;
using SortIt.Views;

namespace SortIt
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()               
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("TitilliumWeb-Regular.ttf", "Titillium");
                });
            builder.Services.AddSingleton<AudioService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
