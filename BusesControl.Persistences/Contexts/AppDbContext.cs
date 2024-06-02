using BusesControl.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<BusModel> Buses { get; set; }
}
