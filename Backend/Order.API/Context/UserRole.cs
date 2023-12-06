using Microsoft.AspNetCore.Identity;

namespace Order.API.Context
{
    /// <summary>
    /// The relation between users their roles.
    /// </summary>
    public class UserRole : IdentityUserRole<int>
    {
        /// <summary>
        /// Get the default relations between the default users and the roles. The relation for an admin account is included.
        /// </summary>
        /// <returns>
        /// A list of the default relations between the default users and the roles.
        /// </returns>
        public static IEnumerable<UserRole> GetUserRoles()
        {
            return new List<UserRole>
            {
                new()
                {
                    RoleId = 1,
                    UserId = 1
                }
            };
        }
    }
}