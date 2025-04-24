using MAUI_finalProject.Services;
using Microsoft.Extensions.Logging;


namespace MAUI_finalProject
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();

            builder.Services.AddSingleton<DBConnection>(s =>
                         new DBConnection(Path.Combine(FileSystem.AppDataDirectory, "locations.db")));

            builder.Services.AddSingleton<LocationService>();

#endif
            return builder.Build();
        }
    }
}