using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class ContractRepository(
    AppDbContext context
) : IContractRepository
{
    private readonly AppDbContext _context = context;

    public async Task<ContractModel?> GetByIdAsync(Guid id)
    {
        return await _context.Contracts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ContractModel?> GetByIdWithCustomersContractAsync(Guid id)
    {
        return await _context.Contracts.AsNoTracking().Include(x => x.CustomersContract).ThenInclude(x => x.Customer).SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ContractModel?> GetByIdWithSettingPanelAsync(Guid id)
    {
        return await _context.Contracts.Include(x => x.SettingPanel).AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ContractModel?> GetByIdWithIncludesAsync(Guid id)
    {
        var query = _context.Contracts.AsNoTracking();

        query = query.Include(x => x.Bus)
                     .Include(x => x.Driver)
                     .Include(x => x.SettingPanel)
                     .Include(x => x.ContractDescription)
                     .Include(x => x.Approver)
                     .Include(x => x.CustomersContract)
                     .ThenInclude(x => x.Customer);

        return await query.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<ContractModel>> FindByOptionalStatusAsync(int page = 0, int pageSize = 0, ContractStatusEnum? status = null)
    {
        var query = _context.Contracts.AsNoTracking();

        if (status is not null)
        {
            query = query.Where(x => x.Status == status);
        }

        if (page > 0 && pageSize > 0)
        {
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        return await query.ToListAsync();
    }

    public async Task<bool> CreateAsync(ContractModel record)
    {
        await _context.Contracts.AddAsync(record);
        return true;
    }

    public bool Update(ContractModel record)
    {
        _context.Contracts.Update(record);
        return true;
    }

    public bool Delete(ContractModel record)
    {
        _context.Contracts.Remove(record);
        return true;
    }

    public async Task<bool> ExistsInIsApprovedBySettingPanelAsync(Guid settingPanelId)
    {
        return await _context.Contracts.AnyAsync(x => x.SettingPanelId == settingPanelId && x.IsApproved == true);
    }

    public async Task<bool> ExistsBySettingPanelAsync(Guid settingPanelId)
    {
        return await _context.Contracts.AnyAsync(x => x.SettingPanelId == settingPanelId);
    }

    public async Task<bool> ExistsInIsApprovedByContractDescriptionAsync(Guid contractDescriptionId)
    {
        return await _context.Contracts.AnyAsync(x => x.ContractDescriptionId == contractDescriptionId && x.IsApproved == true);
    }

    public async Task<bool> ExistsByContractDescriptionAsync(Guid contractDescriptionId)
    {
        return await _context.Contracts.AnyAsync(x => x.ContractDescriptionId == contractDescriptionId);
    }

    public async Task<bool> ExistsByReferenceAsync(string reference)
    {
        return await _context.Contracts.AnyAsync(x => x.Reference == reference);
    }
}
