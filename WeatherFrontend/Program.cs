using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using WeatherFrontend.Services;
using Weatherfrontend;
using Blazored.LocalStorage;
using WeatherFrontend.Models;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();


builder.Services.AddSingleton(sp =>
{
    var configuration = builder.Configuration;
    return configuration.GetSection("ApiSettings").Get<ApiSettings>();
});


builder.Services.AddScoped(sp =>
{
    var apiSettings = sp.GetRequiredService<ApiSettings>();
    var baseAddress = new Uri($"{apiSettings.BaseAddress}{apiSettings.Version}/");
    return new HttpClient { BaseAddress = baseAddress };
});

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7125/") });


// Add Authentication services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<WeatherService>();
builder.Services.AddScoped<UserHistoryService>();

builder.Services.AddScoped<IFavoriteCityService, FavoriteCityService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices();

builder.Services.AddMudServices(config =>
{
    config.PopoverOptions.ThrowOnDuplicateProvider = false;
});

await builder.Build().RunAsync();  


