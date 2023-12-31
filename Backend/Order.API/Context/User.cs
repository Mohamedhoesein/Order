using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// An account of a user of the system.
    /// </summary>
    public class User : IdentityUser<int>
    {
        /// <summary>
        /// The name of the user.
        /// </summary>
        [PersonalData]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// The full address of the user.
        /// </summary>
        [PersonalData]
        [Required]
        public string Address { get; set; }
        /// <summary>
        /// The new email of the user that is not specified when updating an account, but has not yet verified.
        /// </summary>
        [PersonalData]
        public string? TempEmail { get; set; }

        /// <summary>
        /// Get the default users of the system. One admin account is included.
        /// </summary>
        /// <returns>
        /// The default users of the system.
        /// </returns>
        public static IEnumerable<User> GetUsers()
        {
            return new List<User>
            {
                new()
                {
                    Id = 1,
                    Name = "TempAdmin",
                    Address = "Address",
                    UserName = "test@test.com",
                    NormalizedUserName = "TEST@TEST.COM",
                    Email = "test@test.com",
                    NormalizedEmail = "TEST@TEST.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEIj9FzLn96pa8OlStBMrYAgEUenp56bUarbToERhE5NPCTn1EDiBdw7ff0VDJiUjnA==", // TestPassword123!@#
                    SecurityStamp = "BVVSLPHREZXATUD2QOIVYZS6FZYNJRJY",
                    ConcurrencyStamp = "4e1124c2-4ef4-40a3-b211-2e9fa2b0099f"
                }
            };
        }
    }
}