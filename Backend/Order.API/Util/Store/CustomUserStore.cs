using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Order.API.Context;

namespace Order.API.Util.Store
{
    /// <summary>
    /// A <see cref="UserStore{TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim}"/>
    /// wrapper to have a easy reference to it without the need for the generics.
    /// </summary>
    public class CustomUserStore : UserStore<User, Role, OrderContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
    {
        /// <summary>
        /// Initialize a new <see cref="CustomUserStore"/> with the required information.
        /// </summary>
        /// <param name="context">
        /// The <see cref="OrderContext"/> to use.
        /// </param>
        /// <param name="describer">
        /// The <see cref="IdentityErrorDescriber"/> to use.
        /// </param>
        public CustomUserStore(OrderContext context, IdentityErrorDescriber? describer = null) : base(context, describer) {}
    }
}