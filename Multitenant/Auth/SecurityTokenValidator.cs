using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Multitenant.Multitenancy;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Multitenant.Auth
{
    public class SecurityTokenValidator : ISecurityTokenValidator
    {
        private readonly IServiceProvider serviceProvider;

        public SecurityTokenValidator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; } = int.MaxValue;

        public bool CanReadToken(string securityToken) => true;

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var tenant = scope.ServiceProvider.GetService<Tenant>();
                validationParameters.ValidateIssuer = tenant.AuthenticationSettings.ValidateIssuer;
                validationParameters.ValidIssuer = tenant.AuthenticationSettings.ValidIssuer;
                validationParameters.ValidateAudience = tenant.AuthenticationSettings.ValidateAudience;
                validationParameters.ValidAudience = tenant.AuthenticationSettings.ValidAudience;
                validationParameters.ValidateLifetime = tenant.AuthenticationSettings.ValidateLifetime;
            }

            var handler = new JwtSecurityTokenHandler();
            try
            {
                return handler.ValidateToken(securityToken, validationParameters, out validatedToken);
            }
            catch (Exception ex)
            {

            }

            validatedToken = null;
            return null;
        }
    }
}
