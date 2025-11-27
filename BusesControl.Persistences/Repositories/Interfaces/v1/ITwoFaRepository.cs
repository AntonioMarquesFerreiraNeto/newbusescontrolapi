using BusesControl.Entities.Models.v1;

namespace BusesControl.Persistence.Repositories.Interfaces.v1;

public interface ITwoFaRepository : IGenericRepository<TwoFaModel>
{
    Task<bool> ExistsAsync(string code);
    Task<bool> ExistsTokenValidAsync(string email, string ipLocation);
    Task<TwoFaModel?> GetByCodeAsync(string code);
    Task<TwoFaModel?> GetByTokenAsync(string? token);
    Task<TwoFaModel?> GetByUserAsync(string email, string ipLocation);
}
