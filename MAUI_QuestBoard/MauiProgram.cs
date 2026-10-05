using Microsoft.Extensions.Logging;

namespace MAUI_QuestBoard
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

                    // QuestBoard typography - heading font: Cinzel
                    fonts.AddFont("Cinzel-Regular.ttf", "CinzelRegular");
                    fonts.AddFont("Cinzel-SemiBold.ttf", "CinzelSemiBold");
                    fonts.AddFont("Cinzel-Bold.ttf", "CinzelBold");

                    // QuestBoard typography - body font: Nunito
                    fonts.AddFont("Nunito-Regular.ttf", "NunitoRegular");
                    fonts.AddFont("Nunito-Bold.ttf", "NunitoBold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
