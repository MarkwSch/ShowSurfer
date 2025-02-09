using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ShowSurfer.Pages;
using ShowSurfer.Services;
using ShowSurfer.ViewModels;

namespace ShowSurfer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		    builder.Logging.AddDebug();
#endif
            builder.Services.AddHttpClient(TmdbService.TmdbHttpClientName, 
                httpClient => httpClient.BaseAddress = new Uri("https://api.themoviedb.org"));
            builder.Services.AddSingleton<TmdbService>();
            builder.Services.AddSingleton<TopRatedViewModel>();
            builder.Services.AddSingleton<MovieDetailsViewModel>();
            builder.Services.AddSingleton<GenreViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<GenreCategoriesPage>();
        
            return builder.Build();
        }
    }
}