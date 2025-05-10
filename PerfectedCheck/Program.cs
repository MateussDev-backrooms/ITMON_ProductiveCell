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
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;             
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


// Notes

app.MapControllerRoute(
    name: "note",
    pattern: "note",
    defaults: new { controller = "Note", action = "ViewNote" });
app.MapControllerRoute(
    name: "my_notes",
    pattern: "my_notes",
    defaults: new { controller = "Note", action = "BrowseNotes" });
app.MapControllerRoute(
    name: "edit_note",
    pattern: "edit_note",
    defaults: new { controller = "Note", action = "Edit" });
app.MapControllerRoute(
    name: "delete",
    pattern: "delete",
    defaults: new { controller = "Note", action = "Delete" });

//Authentication

app.MapControllerRoute(
    name: "login",
    pattern: "login",
    defaults: new { controller = "Account", action = "Login" });
app.MapControllerRoute(
    name: "register",
    pattern: "register",
    defaults: new { controller = "Account", action = "Register" });
app.MapControllerRoute(
    name: "logout",
    pattern: "logout",
    defaults: new { controller = "Account", action = "Logout" });

//Default routing

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
