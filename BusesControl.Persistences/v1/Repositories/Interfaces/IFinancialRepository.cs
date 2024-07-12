using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IFinancialRepository
{
    Task<bool> CreateAsync(FinancialModel record);
    Task<bool> CreateRangeAsync(IEnumerable<FinancialModel> records);
    bool Update(FinancialModel record);
}
