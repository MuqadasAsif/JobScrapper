using JobScrapperMVC.Data;
using Microsoft.EntityFrameworkCore;
using JobScrapperMVC.Application.Create;
using JobScrapperMVC.Infrastructure.Create;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<IAPIService, APIService>();
builder.Services.AddScoped<IRepositoryService, RepositoryService>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=JobView}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
