using Microsoft.Extensions.Configuration;
using Serilog;
using Troonch.RetailSales.Product.Application;

var builder = WebApplication.CreateBuilder(args);
    
// Serilog cofiguration
builder.Host.UseSerilog((context, configuration) =>
                        configuration.ReadFrom.Configuration(context.Configuration));


// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Retail Sales Product Service
builder.Services.AddRetailSalesProductApplication(builder.Configuration);

var app = builder
    .Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseStatusCodePagesWithRedirects("/Error/{0}");
}





app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
