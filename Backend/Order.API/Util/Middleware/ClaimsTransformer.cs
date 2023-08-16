using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Order.API.Context;
using Order.API.Util.Store;

namespace Order.API.Util.Middleware
{
    /// <summary>
    /// A claims transformer to add all the claims associated with the permissions to the <see cref="ClaimsPrincipal"/>.
    /// </summary>
    public class ClaimsTransformer : IClaimsTransformation
    {
        private readonly CustomUserManager _userManager;

        /// <summary>
        /// Initialize a new <see cref="ClaimsTransformer"/> with the required information.
        /// </summary>
        /// <param name="userManager">
        /// The <see cref="CustomUserManager"/> to use.
        /// </param>
        public ClaimsTransformer(CustomUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Add the claims associated with the permissions of the user.
        /// This is only done if the <see cref="principal"/> doesn't already have a claim of type <see cref="Claims.Type"/>. 
        /// </summary>
        /// <param name="principal">
        /// The <see cref="ClaimsPrincipal"/> to update.
        /// </param>
        /// <returns>
        /// The <see cref="ClaimsPrincipal"/> with the new claims.
        /// </returns>
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Claims.Any(claim => claim.Type == Claims.Type))
                return Task.FromResult(principal);
            var userTask = _userManager.GetUserAsync(principal);
            userTask.Wait();
            var claimsTask = _userManager.GetAllClaims(userTask.Result);
            claimsTask.Wait();

            var identity = new ClaimsIdentity();
            identity.AddClaims(claimsTask.Result);
            principal.AddIdentity(identity);
            return Task.FromResult(principal);
        }
    }
}