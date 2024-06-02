using Microsoft.Extensions.Configuration;
using Serilog;
using Troonch.RetailSales.Product.Application;
using Troonch.User.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Troonch.Retail.App;
using Troonch.User.DataAccess.DataContext;
using Troonch.User.Domain;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TroonchRetailAppContextConnection") ?? throw new InvalidOperationException("Connection string 'TroonchRetailAppContextConnection' not found.");

//builder.Services.AddDbContext<UserDataContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<UserDataContext>();
    
// Serilog cofiguration
builder.Host.UseSerilog((context, configuration) =>
                        configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddRazorPages();
builder.Services.AddControllers();



// Add Retail Sales Product Service
builder.Services.AddRetailSalesProductApplication(builder.Configuration);

// Add User Service
builder.Services.AddUserDataAccess(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();

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


app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    //endpoints.MapControllers();

    endpoints.MapControllerRoute(
        name:"default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("Identity/Account/Login");
        return Task.CompletedTask;
    });
});


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();


