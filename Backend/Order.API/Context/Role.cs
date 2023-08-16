using Microsoft.AspNetCore.Identity;

namespace Order.API.Context
{
    /// <summary>
    /// The role of a user.
    /// </summary>
    public class Role : IdentityRole<int>
    {
        /// <summary>
        /// The role for all employees.
        /// </summary>
        public static Role Employee = new(1, "Employee", "EMPLOYEE");
        /// <summary>
        /// The role for the end users.
        /// </summary>
        public static Role User = new(2, "User", "USER");

        /// <summary>
        /// Initialize a new <see cref="Role"/> with an id, name, and normalized name.
        /// </summary>
        /// <param name="id">
        /// The id of the role used in the database.
        /// </param>
        /// <param name="name">
        /// The nice name of the role.
        /// </param>
        /// <param name="normalizedName">
        /// The normalized name of the role for easy comparison.
        /// </param>
        public Role(int id, string name, string normalizedName) : base(name)
        {
            Id = id;
            NormalizedName = normalizedName;
        }

        /// <summary>
        /// Get all the roles that are available.
        /// </summary>
        /// <returns>
        /// A list of all the roles.
        /// </returns>
        public static IEnumerable<Role> GetRoles()
        {
            return new List<Role>
            {
                Employee,
                User
            };
        }
    }
}