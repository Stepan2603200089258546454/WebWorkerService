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

// ������������ ����������� ���������� ������������� ��� web-api
builder.Services.AddControllers(); //api
// ������������ Jobs
builder.AddWorkers();
// ������ ���������� UI
builder.Services.AddSingleton<AppConfiguration>();
// ����������� HttpClient ��� WASM
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
        .WithSidebar(true) //������� ����
        .WithTheme(ScalarTheme.Kepler) //����
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient) //�� ��������� ��� ������ ����� ������������ ���
        .WithDarkMode(true) //������ ����
        .WithDarkModeToggle(true) //����� ������/������� ����
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

// ��������� ������
await app.RunWorkersAsync();
app.Run();
