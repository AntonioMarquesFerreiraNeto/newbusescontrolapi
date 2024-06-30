using BusesControl.Entities.Models;

namespace BusesControl.Entities.DTOs;

public class ContractGenerationRequestDTO
{
    public CustomerContractModel CustomerContract { get; set; } = default!;
    public int CountCustomers { get; set; }
}
