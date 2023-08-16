using Microsoft.AspNetCore.Identity;

namespace Order.API.Context
{
    /// <summary>
    /// The relation between different users and their claims/permissions.
    /// </summary>
    public class UserClaim : IdentityUserClaim<int>
    {
    }
}