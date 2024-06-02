namespace BusesControl.Business.v1.Interfaces;

public interface IBusBusiness
{
    Task<bool> ExistsByRenavamOrLicensePlateOrChassisAsync(string renavam, string licensePlate, string chassi, Guid? id = null);
}
