using AquariumMVC.functionService;
using AquariumMVC.Interface;
using AquariumMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<aquariumwebsiteContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectonString")));
//註冊IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//註冊使用依賴注入
builder.Services.AddScoped<memberInterface, memberService>();
builder.Services.AddScoped<all_ProductInterface, all_ProductService>();
builder.Services.AddScoped<DeviceInterface, DeviceService>();
builder.Services.AddScoped<FeedsInterface,FeedsService >();
builder.Services.AddScoped<FishInterface, FishService>();
builder.Services.AddScoped<orderInterface, orderService>();
builder.Services.AddScoped<orderdetailInterface, orderdetailService>();
builder.Services.AddScoped<addProductInterface, addproductService>();

//在Asp.Net
//啟動Session ,你必須實現IDistributedCache接口的cache store 來作為session的底層儲存
//然後在ConfigureServices 方法下調用AddSession方法將其塞入IOC容器,
//最後在Startup.Configure 方法下使用UseSession將其塞入到request->response請求管道中
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(20000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


//在Asp.Net Core中使用Session
app.UseHttpsRedirection();
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
