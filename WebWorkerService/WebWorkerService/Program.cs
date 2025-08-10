using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using WebWorkerService.Components;
using WebWorkerService.Configurations;
using WorkersLib;
using WorkersLib.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// регистрируем возможность управления контроллерами для web-api
builder.Services.AddControllers(); //api
// Регистрируем Jobs
builder.AddWorkers();
// конфиг приложения UI
builder.Services.AddSingleton<AppConfiguration>();
// регистрация HttpClient для WASM
builder.Services.AddHttpClient();
//MS openAPI
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

//MS openAPI (/openapi/v1.json)
app.MapOpenApi();
//Scalar (/scalar/v1)
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("Info API")
        .WithSidebar(true) //боковое меню
        .WithTheme(ScalarTheme.Kepler) //тема
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient) //по умолчанию для какого языка генерировать код
        .WithDarkMode(true) //темная тема
        .WithDarkModeToggle(true) //смена темной/светлой темы
        .WithClientButton(true);
});

app.UseHttpsRedirection();

app.UseRouting(); //mvc
app.UseAuthorization(); //mvc + api

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(WebWorkerService.Client._Imports).Assembly);

app.MapControllers(); //api
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets(); //mvc

// Запускаем задачи
await app.RunWorkersAsync();
app.Run();
