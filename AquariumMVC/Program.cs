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
//���UIHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//���U�ϥΨ̿�`�J
builder.Services.AddScoped<memberInterface, memberService>();
builder.Services.AddScoped<all_ProductInterface, all_ProductService>();
builder.Services.AddScoped<DeviceInterface, DeviceService>();
builder.Services.AddScoped<FeedsInterface,FeedsService >();
builder.Services.AddScoped<FishInterface, FishService>();
builder.Services.AddScoped<orderInterface, orderService>();
builder.Services.AddScoped<orderdetailInterface, orderdetailService>();
builder.Services.AddScoped<addProductInterface, addproductService>();

//�bAsp.Net
//�Ұ�Session ,�A������{IDistributedCache���f��cache store �ӧ@��session�����h�x�s
//�M��bConfigureServices ��k�U�ե�AddSession��k�N���JIOC�e��,
//�̫�bStartup.Configure ��k�U�ϥ�UseSession�N���J��request->response�ШD�޹D��
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


//�bAsp.Net Core���ϥ�Session
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