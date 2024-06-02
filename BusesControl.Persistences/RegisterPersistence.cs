using BusesControl.Persistence.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BusesControl.Persistence.v1.Repositories;
using BusesControl.Persistence.v1.Repositories.Interfaces;

namespace BusesControl.Persistence;

public class RegisterPersistence
{
    public static void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBusRepository, BusRepository>();

        var conection = builder.Configuration.GetConnectionString("Connection");
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(conection));
    }
}
