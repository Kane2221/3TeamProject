using _3TeamProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

var TeamProjectconnectionString = builder.Configuration.GetConnectionString("Team3ProjectOnline");
builder.Services.AddDbContext<_3TeamProjectContext>(options =>
    options.UseSqlServer(TeamProjectconnectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

//新增Cookie驗證
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt => {
        opt.AccessDeniedPath = "/Home/Error";
        opt.LoginPath = "/Home"; // TODO Login Path
        opt.ExpireTimeSpan = TimeSpan.FromSeconds(600);
    });

//新增的服務，為了在串接兩張以上的表，不重覆讀取
builder.Services.AddMvc()
    .AddNewtonsoftJson(options => 
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    );

var app = builder.Build();

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

app.UseAuthentication();

app.UseAuthorization();

//新增Suppliers Area 的 Route
//app.MapControllerRoute(
//    name: "Members",
//    pattern: "{area:exists}/{controller=Member}/{action=Index}/{id?}");

////新增Suppliers Area 的 Route
//app.MapControllerRoute(
//    name: "Suppliers",
//    pattern: "{area:exists}/{controller=Supplier}/{action=Index}/{id?}");

////新增Administrators Area 的 Route
//app.MapControllerRoute(
//    name: "Administrators",
//    pattern: "{area:exists}/{controller=Administrator}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
