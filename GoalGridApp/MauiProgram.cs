using Microsoft.Extensions.Logging;
using GoalGridApp.Services;
namespace GoalGridApp
{
    public static class MauiProgram
    {
        //        public static MauiApp CreateMauiApp()
        //        {
        //            var builder = MauiApp.CreateBuilder();
        //            builder
        //                .UseMauiApp<App>()
        //                .ConfigureFonts(fonts =>
        //                {
        //                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        //                });

        //            builder.Services.AddMauiBlazorWebView();

        //#if DEBUG
        //    		builder.Services.AddBlazorWebViewDeveloperTools();
        //    		builder.Logging.AddDebug();
        //#endif

        //            return builder.Build();
        //        }

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            // Registra tu servicio de almacenamiento de archivos JSON como un Singleton
            builder.Services.AddSingleton<JsonFileStorageService>();
            return builder.Build();
        }

    }
}
