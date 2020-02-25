using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Multitenant.Auth
{
    public class CustomJwtBearerOptionsPostConfigureOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly SecurityTokenValidator tokenValidator; //example dependency

        public CustomJwtBearerOptionsPostConfigureOptions(SecurityTokenValidator tokenValidator)
        {
            this.tokenValidator = tokenValidator;
        }

        public void PostConfigure(string name, JwtBearerOptions options)
        {
            //options.Authority = "https://securetoken.google.com/multitenant-baa10";
            options.SecurityTokenValidators.Clear();
            options.SecurityTokenValidators.Add(tokenValidator);
        }
    }
}
