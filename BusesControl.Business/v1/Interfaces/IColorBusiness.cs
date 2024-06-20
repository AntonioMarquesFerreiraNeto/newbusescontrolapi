namespace BusesControl.Business.v1.Interfaces;

public interface IColorBusiness
{
    Task<bool> ValidateActiveAsync(Guid id);
}
