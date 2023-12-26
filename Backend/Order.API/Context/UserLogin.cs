using Microsoft.AspNetCore.Identity;

namespace Order.API.Context
{
    /// <summary>
    /// A login of a user and its associated provider.
    /// </summary>
    public class UserLogin : IdentityUserLogin<int>
    {
    }
}