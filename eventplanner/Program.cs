using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Fonts; 
using eventplanner.Interfaces;
using eventplanner.Models;
using eventplanner.Controllers;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPdfService, PdfModel>();
builder.Services.AddScoped<ITempDataService, TempDataService>();
builder.Services.AddHttpContextAccessor();



// Add services to the container.
builder.Services.AddRazorPages();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

var app = builder.Build();


GlobalFontSettings.FontResolver = new CustomFontResolver();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
    pattern: "{controller=PDF}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();