using _3TeamProject.Models;
using _3TeamProject.Service;
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

builder.Services.AddScoped<MailService>();

//CORS
//builder.Services.AddCors(c =>
//    c.AddPolicy(
//    name: "AllowOrigin",
//    policy => policy.WithOrigins("https://localhost:7007").WithHeaders("*").WithMethods("*")));


//�s�WCookie���ҡA�ĤG�q���s�WGoogle�ĤT��n�J�{�ҡC
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt => {
        opt.LoginPath = "/Home";
        opt.ExpireTimeSpan = TimeSpan.FromMinutes(180);
        opt.LogoutPath = "/Home";
        opt.AccessDeniedPath = "/Home";
        opt.Events = new CookieAuthenticationEvents {
            OnRedirectToLogin = context => {
                // 如果未經身份驗證的用戶訪問/backstage/以下的網頁，則重定向到/backstage/login
                if (context.Request.Path.StartsWithSegments("/backstage"))
                {
                    context.Response.Redirect("/backstage/login");
                }
                // 如果未經身份驗證的用戶訪問/member/以下的網頁，則重定向到/member/login
                else if (context.Request.Path.StartsWithSegments("/member"))
                {
                    context.Response.Redirect("/member/login");
                }
                // 如果未經身份驗證的用戶訪問/supplier/以下的網頁，則重定向到/suppier/login
                else if (context.Request.Path.StartsWithSegments("/Supplier"))
                {
                    context.Response.Redirect("/Supplier/login");
                }

                return Task.CompletedTask;
            }
        };
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

builder.Services.AddSession(
    (opt) =>
    {
        opt.IdleTimeout = TimeSpan.FromMinutes(10);
    });

//�s�W���A�ȡA���F�b�걵��i�H�W�����A������Ū��
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
//app.UseCors();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
