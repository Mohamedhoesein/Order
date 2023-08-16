using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Order.API.Context;

namespace Order.API.Util.Policy
{
    /// <summary>
    /// A class that contains the logic to create the policies.
    /// </summary>
    public static class PolicyCreator
    {
        /// <summary>
        /// Create the policies for each of the permissions.
        /// Two policies are created for each permission,
        /// one policy requires the user to have the permission,
        /// and the other policy requires the user to not have the permission.
        /// </summary>
        /// <param name="options"></param>
        public static void CreateClaimPolicies(AuthorizationOptions options)
        {
            var claims = typeof(Claims)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(property => property.GetValue(null) as Claim ?? new Claim("Error", "Error"))
                .Where(property => property.Type != "Error" && property.Value != "Error");

            foreach (var claim in claims)
            {
                options.AddPolicy(
                    claim.Value,
                    policy => policy.RequireClaim(claim.Type, claim.Value)
                );
                options.AddPolicy(
                    $"!{claim.Value}",
                    policy =>
                        policy.Requirements.Add(
                            new HasNotPermissionRequirement(claim.Value, claim.Value)
                        )
                );
            }
        }
    }
}