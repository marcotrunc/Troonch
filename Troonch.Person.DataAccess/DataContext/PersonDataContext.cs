using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Troonch.Person.Domain.Entities;

namespace Troonch.Person.DataAccess.DataContext;

public class PersonDataContext : DbContext
{
    public PersonDataContext(DbContextOptions<PersonDataContext> options) : base(options)
    {
    }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<AddressType> AddresTypes { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
