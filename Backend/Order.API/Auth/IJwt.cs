using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Order.API.Auth
{
    /// <summary>
    /// Helper interface for handling jwts.
    /// </summary>
    public interface IJwt
    {
        /// <summary>
        /// Generate a jwt based on the given information.
        /// </summary>
        /// <param name="claims">
        /// The claims of the account to be logged in.
        /// </param>
        /// <param name="issuer">
        /// The issuer that should be present.
        /// </param>
        /// <param name="audience">
        /// For whom to generate a jwt.
        /// </param>
        /// <returns>
        /// The jwt string.
        /// </returns>
        public string GenerateToken(List<Claim> claims, string issuer, string audience);

        /// <summary>
        /// Decode the given jwt string.
        /// </summary>
        /// <param name="token">
        /// The jwt to decode.
        /// </param>
        /// <returns>
        /// An object representing the jwt.
        /// </returns>
        public JwtSecurityToken DecodeToken(string token);

        /// <summary>
        /// Validate the given token.
        /// </summary>
        /// <param name="token">
        /// Token to validate
        /// </param>
        /// <param name="expectedIssuer">
        /// The issuer that should be present.
        /// </param>
        /// <param name="expectedAudience">
        /// The list of possible audiences.
        /// </param>
        /// <returns>
        /// If the token is valid.
        /// </returns>
        public bool ValidateToken(string token, string expectedIssuer, IEnumerable<string> expectedAudience);

        /// <summary>
        /// Create the token validation parameters.
        /// </summary>
        /// <param name="expectedIssuer">
        /// The issuer that should be present.
        /// </param>
        /// <param name="expectedAudience">
        /// The list of possible audiences.
        /// </param>
        /// <returns>
        /// The token validation parameters.
        /// </returns>
        public TokenValidationParameters GetValidationParameters(string expectedIssuer, IEnumerable<string> expectedAudience);
    }
}