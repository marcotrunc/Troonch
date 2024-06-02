using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Troonch.User.Domain;

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
        admin.NormalizedName = "Administrator";

        var user = new IdentityRole("user");
        user.NormalizedName = "User";

        builder.Entity<IdentityRole>().HasData(admin, user);
    }
}
