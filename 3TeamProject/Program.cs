using _3TeamProject.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//連線資料庫(測試期間請檢查appsettings.json裡的連線字串，與自己的資料庫連線是否一致。)
builder.Services.AddDbContext<_3TeamProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Team3Project")));

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

//新增Suppliers Area 的 Route
app.MapControllerRoute(
    name: "Suppliers",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
