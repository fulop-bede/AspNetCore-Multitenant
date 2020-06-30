using Autofac.Features.Indexed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Multitenant.Authentication;
using Multitenant.Extensions;
using Multitenant.FeatureHandling;
using Multitenant.Multitenancy;
using Multitenant.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Multitenant.Dal.ApplicationDbContext;

namespace Multitenant.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.Firebase)]
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private readonly ITestService testService;
        private readonly IDbUsingService dbUsingService;

        public TestController(
            Tenant tenant,
            IIndex<string, ITestService> testServices,
            IDbUsingService dbUsingService)
        {
            this.testService = testServices.GetImplementation(tenant);
            this.dbUsingService = dbUsingService;
        }

        [FeatureGate(FeatureFlags.FancyNewFeatureFlag)]
        [HttpGet("tenant-specific-service")]
        public string GetServiceName2()
        {
            return testService.GetName();
        }

        [FeatureGate(FeatureFlags.RestrictedFeatureFlag)]
        [HttpGet("tenant-specific-db")]
        public Task<List<CommonEntity>> GetEntities()
        {
            return dbUsingService.GetEntities();
        }
    }
}
