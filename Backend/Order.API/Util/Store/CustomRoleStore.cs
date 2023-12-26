using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Order.API.Context;

namespace Order.API.Util.Store
{
    /// <summary>
    /// A <see cref="RoleStore{TRole, TContext, TKey, TUserRole, TRoleClaim}"/> wrapper to have a easy reference to it
    /// without the need for the generics.
    /// </summary>
    public class CustomRoleStore : RoleStore<Role, OrderContext, int, UserRole, RoleClaim>
    {
        /// <summary>
        /// Initialize a new <see cref="CustomRoleStore"/> with the required information.
        /// </summary>
        /// <param name="context">
        /// The <see cref="OrderContext"/> to use.
        /// </param>
        /// <param name="describer">
        /// The <see cref="IdentityErrorDescriber"/> to use.
        /// </param>
        public CustomRoleStore(OrderContext context, IdentityErrorDescriber? describer = null) : base(context, describer){}
    }
}