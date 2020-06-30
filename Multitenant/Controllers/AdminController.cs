using Autofac.Features.Indexed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Multitenant.Authentication;
using Multitenant.Extensions;
using Multitenant.FeatureHandling;
using Multitenant.Multitenancy;
using Multitenant.Services;

namespace Multitenant.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.IdentityServer4)]
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly ITestService testService;

        public AdminController(
            Tenant tenant,
            IIndex<string, ITestService> testServices)
        {
            this.testService = testServices.GetImplementation(tenant);
        }

        [FeatureGate(FeatureFlags.FancyNewFeatureFlag)]
        [HttpGet("tenant-specific-service")]
        public string GetServiceName()
        {
            return testService.GetName();
        }
    }
}
