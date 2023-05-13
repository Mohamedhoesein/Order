using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Order.API.Auth
{
    /// <summary>
    /// Extensions for the <see cref="AuthenticationBuilder"/> class.
    /// </summary>
    public static class AuthenticationBuilderExtensions
    {
        /// <summary>
        /// Add jwt authentication configurations.
        /// </summary>
        /// <param name="builder">
        /// The authentication builder to use.
        /// </param>
        /// <param name="configuration">
        /// The configuration to use.
        /// </param>
        /// <param name="issuer">
        /// The issuer who issued a jwt.
        /// </param>
        /// <param name="audiences">
        /// For whom the jwt is.
        /// </param>
        /// <returns>
        /// The authentication builder with the new configurations.
        /// </returns>
        public static AuthenticationBuilder AddJwtBearerConfiguration(this AuthenticationBuilder builder, IConfiguration configuration, string issuer, params string[] audiences)
        {
            return builder.AddJwtBearer(options => {
                options.TokenValidationParameters = new Jwt(configuration).GetValidationParameters(issuer, audiences);
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.IncludeErrorDetails = true;

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        if (string.IsNullOrEmpty(context.Error))
                            context.Error = "invalid_token";
                        if (string.IsNullOrEmpty(context.ErrorDescription))
                            context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                        if (context.AuthenticateFailure is SecurityTokenExpiredException authenticationException)
                        {
                            context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
                            context.ErrorDescription = $"The token expired on {authenticationException.Expires:o}";
                        }

                        return context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            error = context.Error,
                            error_description = context.ErrorDescription
                        }));
                    }
                };
            });
        }
    }
}