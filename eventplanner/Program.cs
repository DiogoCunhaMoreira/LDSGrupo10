using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using eventplanner.Data;
using PdfSharp.Fonts; 
using eventplanner.Interfaces;
using eventplanner.Models;
using eventplanner.Controllers;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPdfService, PdfModel>();
builder.Services.AddScoped<ITempDataService, TempDataService>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
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

