namespace BusesControl.Business.v1.Interfaces;

public interface IUserBusiness
{
    Task<bool> ValidateForCreateAsync(string email, string role);
}
