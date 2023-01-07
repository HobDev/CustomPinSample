
#if ANDROID
using CustomizedMap.Platforms.Android;
#endif
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace CustomizedMap
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitMarkup()
                .UseMauiCommunityToolkit()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MapPage>();
            builder.Services.AddSingleton<MapViewModel>();
           

#if DEBUG
		builder.Logging.AddDebug();
#endif
            builder.ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
#endif
            });



            return builder.Build();
        }
    }
}