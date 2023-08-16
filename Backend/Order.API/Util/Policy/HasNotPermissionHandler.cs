using Microsoft.AspNetCore.Authorization;
using Order.API.Util.Store;

namespace Order.API.Util.Policy
{
    /// <summary>
    /// A <see cref="AuthorizationHandler{TRequirement}"/> for policies that require a logged in user to not have a permission.
    /// </summary>
    public class HasNotPermissionHandler : AuthorizationHandler<HasNotPermissionRequirement>
    {
        private readonly CustomUserManager _userManager;

        /// <summary>
        /// Initialize a new <see cref="HasNotPermissionHandler"/> with the required information.
        /// </summary>
        /// <param name="userManager">
        /// The <see cref="CustomUserManager"/> to use.
        /// </param>
        public HasNotPermissionHandler(CustomUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Check if a user hasn't gotten the specified permission.
        /// </summary>
        /// <param name="context">
        /// The <see cref="AuthorizationHandlerContext"/> containing the user.
        /// </param>
        /// <param name="requirement">
        /// The <see cref="HasNotPermissionRequirement"/> containing the claim that must be missing.
        /// </param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasNotPermissionRequirement requirement)
        {
            var userTask = _userManager.GetUserAsync(context.User);
            userTask.Wait();
            if (userTask.Result == null)
                return Task.CompletedTask;
            var claimTask = _userManager.GetAllClaims(userTask.Result);
            claimTask.Wait();

            var claims = claimTask.Result;
            if (claims.All(claim => claim.Type != requirement.Type && claim.Value != requirement.Value))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}