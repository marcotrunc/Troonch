using Microsoft.EntityFrameworkCore;
using Serilog;
using Troonch.EmailSender.MailTrap;
using Troonch.EmailSender.Rdcom.Domain.Configuration;
using Troonch.Retail.App.Middlewares;
using Troonch.User.Presentation;
using Troonch.RetailSales.Product.Presentation;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TroonchRetailAppContextConnection") ?? throw new InvalidOperationException("Connection string 'TroonchRetailAppContextConnection' not found.");



//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<UserDataContext>();
    
// Serilog cofiguration
builder.Host.UseSerilog((context, configuration) =>
                        configuration.ReadFrom.Configuration(context.Configuration));

// Add Retail Sales Product Service
builder.Services.AddRetailSalesProductPresentation(builder.Configuration);

// Add Auth and User Service
builder.Services.AddUserPresentation(builder.Configuration);

// Add Rdcom Email Sender Privider
builder.Services.Configure<RdcomConfiguration>(builder.Configuration.GetSection("RdcomConfiguration"));
builder.Services.AddEmailSenderMailTrap(builder.Configuration);


//builder.Services.AddRazorPages();
//builder.Services.AddControllers();






// Add services to the container.
//builder.Services.AddControllersWithViews();

var app = builder
    .Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseStatusCodePagesWithRedirects("/Error/{0}");
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}
else
{
    app.UseStatusCodePagesWithRedirects("/Error/{0}");
}
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name:"default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("/Auth/Login");
        return Task.CompletedTask;
    });
    
    endpoints.MapRazorPages();


});


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();


