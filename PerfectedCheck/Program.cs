using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PerfectedCheck.Data;
using PerfectedCheck.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ProductiveCellDBContext>();
builder.Services.AddIdentity<UserModel, IdentityRole>()
    .AddEntityFrameworkStores<ProductiveCellDBContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseAuthorization();

//app.MapStaticAssets();
app.MapControllerRoute(
    name: "note",
    pattern: "note",
    defaults: new { controller = "Note", action = "ViewNote" });

app.MapControllerRoute(
    name: "my_notes",
    pattern: "my_notes",
    defaults: new { controller = "Note", action = "BrowseNotes" });


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
