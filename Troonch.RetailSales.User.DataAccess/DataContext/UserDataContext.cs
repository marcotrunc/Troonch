using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Troonch.User.Domain.Entities;

namespace Troonch.User.DataAccess.DataContext;

public class UserDataContext : IdentityDbContext<ApplicationUser>
{
    public UserDataContext(DbContextOptions<UserDataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var admin = new IdentityRole("admin");
        admin.Id = "4d81368d-983a-4441-b25a-9a1fec482529";
        admin.NormalizedName = "Administrator";

        var user = new IdentityRole("user");
        user.Id = "0ae8de66-8ef8-4705-8f03-be13db5b7c6c";
        user.NormalizedName = "User";

        var sysAdmin = new IdentityRole("systemAdmin");
        sysAdmin.Id = "50d59b6f-670d-4f05-b98f-988f91f24bfe";
        sysAdmin.NormalizedName = "systemAdministrator";

        builder.Entity<IdentityRole>().HasData(admin, user, sysAdmin);

        var systemAdmin = new ApplicationUser();
        systemAdmin.Id = "0ae8de66-8ee8-4715-8f23-be13db5b7a6c";
        systemAdmin.Name = "System";
        systemAdmin.LastName = "Admin";
        systemAdmin.DateOfBirth = DateOnly.Parse("29-08-1995");
        systemAdmin.CreatedOn = DateTime.UtcNow;
        systemAdmin.UpdatedOn = DateTime.UtcNow;
        systemAdmin.UserName = "marco.truncellito@outlook.it";
        systemAdmin.Email = "marco.truncellito@outlook.it";
        systemAdmin.EmailConfirmed = true;
        systemAdmin.PasswordHash = "AQAAAAIAAYagAAAAEIvJowhfExz9jEW1EgOLUJvQ2GIN3Yg7ayN2LuRJQclkvGDCmyTO8mhq+tomdYstkw==";
        
        builder.Entity<ApplicationUser>().HasData(systemAdmin);

        var identiryUserRole = new IdentityUserRole<string>();
        identiryUserRole.UserId = systemAdmin.Id;
        identiryUserRole.UserId = sysAdmin.Id;

        builder.Entity<IdentityUserRole<string>>().HasData(identiryUserRole);
    }
}
