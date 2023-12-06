using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Order.API.Context;

namespace Order.API.Util.Store
{
    /// <summary>
    /// A <see cref="SignInManager{TUser}"/> wrapper to have a easy reference to it without the need for the generics.
    /// </summary>
    public class CustomSignInManager : SignInManager<User>
    {
        /// <summary>
        /// Initialize a new <see cref="CustomSignInManager"/> with the required information.
        /// </summary>
        /// <param name="userManager">
        /// A <see cref="UserManager{TUser}"/> to use.
        /// </param>
        /// <param name="contextAccessor">
        /// A <see cref="IHttpContextAccessor"/> to use.
        /// </param>
        /// <param name="claimsFactory">
        /// A <see cref="IUserClaimsPrincipalFactory{TUser}"/> to use.
        /// </param>
        /// <param name="optionsAccessor">
        /// A <see cref="IOptions{TOptions}"/> to use.
        /// </param>
        /// <param name="logger">
        /// A <see cref="ILogger{TCategoryName}"/> to use.
        /// </param>
        /// <param name="schemes">
        /// A <see cref="IAuthenticationSchemeProvider"/> to use.
        /// </param>
        /// <param name="confirmation">
        /// A <see cref="IUserConfirmation{TUser}"/> to use.
        /// </param>
        public CustomSignInManager(
            UserManager<User> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<User> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<User>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<User> confirmation
        ) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation) {}
    }
}