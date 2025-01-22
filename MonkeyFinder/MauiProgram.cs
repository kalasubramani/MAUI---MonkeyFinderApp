using Microsoft.Extensions.Logging;
using MonkeyFinder.Services;
using MonkeyFinder.View;

namespace MonkeyFinder
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

            //dependency injection for service and viewmodel
            builder.Services.AddSingleton<MonkeyService>();

            builder.Services.AddSingleton<MonkeysViewModel>();
            //create new view model every single time
            builder.Services.AddTransient<MonkeyDetailsViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<DetailsPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
