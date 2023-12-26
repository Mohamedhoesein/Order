using Microsoft.AspNetCore.Identity;

namespace Order.API.Context
{
    /// <summary>
    /// A authentication token for a user.
    /// </summary>
    public class UserToken : IdentityUserToken<int>
    {
    }
}