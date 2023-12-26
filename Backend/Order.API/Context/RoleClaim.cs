using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Order.API.Context
{
    /// <summary>
    /// The relation between different roles and their claims/permissions.
    /// </summary>
    public class RoleClaim : IdentityRoleClaim<int>
    {
        /// <summary>
        /// The permissions for an employee.
        /// </summary>
        public static List<RoleClaim> EmployeeClaims = new()
        {
            CreateRoleClaim(Role.Employee, Claims.Employee),
            CreateRoleClaim(Role.Employee, Claims.AccountEdit),
            CreateRoleClaim(Role.Employee, Claims.AccountDeleteOwn),
            CreateRoleClaim(Role.Employee, Claims.AccountDeleteUser),
            CreateRoleClaim(Role.Employee, Claims.AccountCreateEmployee)
        };

        /// <summary>
        /// The permissions for an end user.
        /// </summary>
        public static List<RoleClaim> Userclaims = new()
        {
            CreateRoleClaim(Role.User, Claims.AccountEdit),
            CreateRoleClaim(Role.User, Claims.AccountDeleteOwn)
        };

        /// <summary>
        /// Get all the claims associated with each of the roles.
        /// </summary>
        /// <returns>
        /// A list of all the claims associated with each of the roles.
        /// </returns>
        public static IEnumerable<RoleClaim> GetRoleClaim()
        {
            var claims = Userclaims.Concat(EmployeeClaims);
            return claims.OrderBy(roleClaim => roleClaim.ClaimValue)
                .Select((roleClaim, id) =>
                {
                    roleClaim.Id = id + 1;
                    return roleClaim;
                }).ToList();
        }

        /// <summary>
        /// Create a <see cref="RoleClaim"/> from a <see cref="Role"/> and <see cref="Claim"/>.
        /// </summary>
        /// <param name="role">
        /// The role to add the claim to.
        /// </param>
        /// <param name="claim">
        /// The claim to add to the role.
        /// </param>
        /// <returns>
        /// The new <see cref="RoleClaim"/>.
        /// </returns>
        private static RoleClaim CreateRoleClaim(Role role, Claim claim)
        {
            return new RoleClaim
            {
                RoleId = role.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };
        }
    }
}