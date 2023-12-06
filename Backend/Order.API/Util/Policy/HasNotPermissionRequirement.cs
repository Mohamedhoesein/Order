using Microsoft.AspNetCore.Authorization;

namespace Order.API.Util.Policy
{
    /// <summary>
    /// A requirement for missing a specific permission.
    /// </summary>
    public class HasNotPermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// The type used in the claims of the user.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The permission that must be missing.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initialize a new <see cref="HasNotPermissionRequirement"/> with the required information.
        /// </summary>
        /// <param name="type">
        /// The type used in the claims of the user.
        /// </param>
        /// <param name="value">
        /// The permission that must be missing.
        /// </param>
        public HasNotPermissionRequirement(string type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}