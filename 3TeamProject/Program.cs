using _3TeamProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var TeamProjectconnectionString = builder.Configuration.GetConnectionString("Team3Project");
builder.Services.AddDbContext<_3TeamProjectContext>(options =>
    options.UseSqlServer(TeamProjectconnectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

//�s�WCookie����
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt => {
        opt.AccessDeniedPath = "/Home/Error";
        opt.LoginPath = "/Home"; // TODO Login Path
        opt.ExpireTimeSpan = TimeSpan.FromSeconds(600);
       
    }).AddGoogle( opt =>
    {
        opt.ClientId= builder.Configuration.GetSection("GoogleOAuth:ID").Value;
        opt.ClientSecret = builder.Configuration.GetSection("GoogleOAuth:Password").Value;
        opt.Events.OnCreatingTicket = ctx =>
        {
            ctx.Identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Role, "Admin")); // Role �W�[ Admin
            return Task.CompletedTask;
        };
    });

//�s�W���A�ȡA���F�b�걵��i�H�W����A������Ū��
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

//�s�WSuppliers Area �� Route
app.MapControllerRoute(
    name: "Suppliers",
    pattern: "{area:exists}/{controller=Supplier}/{action=Index}/{id?}");

//�s�WAdministrators Area �� Route
app.MapControllerRoute(
    name: "Administrators",
    pattern: "{area:exists}/{controller=Administrator}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
