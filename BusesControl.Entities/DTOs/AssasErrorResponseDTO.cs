namespace BusesControl.Entities.DTOs;

public class AssasErrorResponseDTO
{
    public IEnumerable<AssasErrorDetailsDTO> Errors { get; set; } = default!;
}
