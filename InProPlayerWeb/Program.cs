using InProPlayerWeb.Helper;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<PortHelper>();
builder.Services.AddSingleton<NAudioHelper>();

builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlite(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

//»y¨t³]©w
var supportedCultures = new[] { "en-US", "zh-TW" };
IList<CultureInfo> cultures = supportedCultures.Select(name => new CultureInfo(name)).ToList();
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = cultures,
    SupportedUICultures = cultures
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
