using BusesControl.Entities.Responses.v1;
using System.Security.Claims;

namespace BusesControl.Api.Utils
{
    public static class UserAuth
    {
        public static UserAuthResponse Get(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst("UserId");
            var roleClaim = user.FindFirst(ClaimTypes.Role);
            var employeeIdClaim = user.FindFirst("EmployeeId");

            if (userIdClaim is null || roleClaim is null)
                throw new Exception("User ID or role claims are not present in the token.");

            var userId = Guid.Parse(userIdClaim.Value.ToString());
            var role = roleClaim.Value;

            Guid? employeeId = employeeIdClaim is not null ? Guid.Parse(employeeIdClaim.Value.ToString()) : null;

            return new UserAuthResponse(userId, role, employeeId);
        }
    }
}
