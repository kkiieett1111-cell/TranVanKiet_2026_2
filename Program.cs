using Microsoft.EntityFrameworkCore;
using TranVanKiet_2026_2.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<QLTaiSanContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QLTaiSanConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TaiSan}/{action=Index}/{id?}");

app.Run();