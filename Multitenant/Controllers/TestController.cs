using Autofac.Features.Indexed;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Multitenant.Extensions;
using Multitenant.FeatureFilters;
using Multitenant.Multitenancy.Model;
using Multitenant.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Multitenant.Dal.ApplicationDbContext;

namespace Multitenant.Controllers
{
    [FeatureGate(FeatureFlags.RestrictedResource)]
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private readonly ITestService testService;
        private readonly IDbUsingService dbUsingService;
        private readonly IFeatureManager featureManager;

        public TestController(
            Tenant tenant,
            IIndex<string, ITestService> testServices,
            IDbUsingService dbUsingService,
            IFeatureManager featureManager)
        {
            this.testService = testServices.GetImplementation(tenant);
            this.dbUsingService = dbUsingService;
            this.featureManager = featureManager;
        }

        [HttpGet("tenant-specific-service")]
        public async Task<string> GetServiceName()
        {
            var tmp = await featureManager.IsEnabledAsync(FeatureFlags.RestrictedResource);
            return testService.GetName();
        }

        [HttpGet("tenant-specific-db")]
        public Task<List<CommonEntity>> GetEntities()
        {
            return dbUsingService.GetEntities();
        }
    }
}
