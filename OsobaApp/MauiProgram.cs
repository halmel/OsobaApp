using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;
using OsobaApp.Services; // Přidáno pro IPersonValidator a PersonValidator
using OsobaApp.ViewModels; // Přidáno pro PersonViewModel
using OsobaApp.Views; // Přidáno pro MainPage

namespace OsobaApp;

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

        // Registrace služeb do Dependency Injection
        builder.Services.AddSingleton<IPersonValidator, PersonValidator>();  // Přidáno IPersonValidator a PersonValidator
        builder.Services.AddSingleton<IDatabaseService>(s => DatabaseService.GetInstance("person.db"));
        builder.Services.AddSingleton<OsobaViewModel>();  // Přidáno PersonViewModel
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
    }
}
