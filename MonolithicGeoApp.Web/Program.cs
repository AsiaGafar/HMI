using MonolithicGeoApp.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddSingleton<GeoService>();

var app = builder.Build();
app.UseStaticFiles();
app.MapRazorPages();
app.Run();
