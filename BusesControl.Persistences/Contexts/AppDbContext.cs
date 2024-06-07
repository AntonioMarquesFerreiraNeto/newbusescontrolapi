using BusesControl.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Contexts;

public class AppDbContext : IdentityDbContext<UserModel>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<BusModel> Buses { get; set; }
    public DbSet<EmployeeModel> Employees { get; set; }
    public override DbSet<UserModel> Users { get; set; }
}
