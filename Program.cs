using TimeTracker;
using TimeTracker.Components;
using Radzen;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TimeTracker.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents()
    .AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);

//builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();

builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<ProtectedSessionStorage>();

builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "TimeTrackkerTheme";
    options.Duration = TimeSpan.FromDays(365);
});
builder.Services.AddHttpClient();
builder.Services.AddSingleton<Session>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();



//app.MapRazorComponents<App>().AddInteractiveWebAssemblyRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
//app.MapAdditionalIdentityEndpoints();

app.Run();
