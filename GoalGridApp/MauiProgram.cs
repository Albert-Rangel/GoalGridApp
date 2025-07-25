using Microsoft.Extensions.Logging;
using GoalGridApp.Services;
using Radzen;

namespace GoalGridApp
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            // Registra tu servicio de almacenamiento de archivos JSON
            builder.Services.AddSingleton<JsonFileStorageService>();

            // Registra los componentes de la librería Radzen
            builder.Services.AddRadzenComponents();

            return builder.Build();
        }
    }
}
