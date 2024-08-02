using BusesControl.Entities.Enums.v1;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models.v1;

public class UserModel : IdentityUser<Guid>
{
    public override Guid Id { get; set; }
    public Guid? EmployeeId { get; set; }
    public EmployeeModel? Employee { get; set; } = default!;
    [MaxLength(20)]
    public string? Nickname { get; set; } = default!;
    public UserStatusEnum Status { get; set; } = UserStatusEnum.Inactive;
}
