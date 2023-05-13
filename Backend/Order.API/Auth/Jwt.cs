using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Order.API.Auth
{
    /// <summary>
    /// Helper class for handling jwts.
    /// </summary>
    public class Jwt: IJwt
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initialize a new instance of <see cref="Jwt"/>.
        /// </summary>
        /// <param name="configuration">
        /// The configurations for the jwt.
        /// </param>
        public Jwt(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc/>
        public string GenerateToken(List<Claim> claims, string issuer, string audience)
        {
            var key = _configuration.GetValue<string>("JWTKey");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    claims
                ),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
                Audience = audience,
                Issuer = issuer,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <inheritdoc/>
        public JwtSecurityToken DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            return jsonToken as JwtSecurityToken;
        }

        /// <inheritdoc/>
        public bool ValidateToken(string token, string expectedIssuer, IEnumerable<string> expectedAudience)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                handler.ValidateToken(
                    token,
                    GetValidationParameters(expectedIssuer, expectedAudience),
                    out _
                );
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public TokenValidationParameters GetValidationParameters(string expectedIssuer, IEnumerable<string> expectedAudience)
        {
            var key = _configuration.GetValue<string>("JWTKey");
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = expectedIssuer,
                ValidAudiences = expectedAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}