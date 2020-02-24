using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Multitenant.Dal;
using Multitenant.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Multitenant.Dal.BaseDbContext;

namespace Multitenant.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private readonly ITestService testService;
        private readonly IDbUsingService dbUsingService;
        private readonly BaseDbContext baseContext;

        public TestController(
            ITestService testService,
            IDbUsingService dbUsingService,
            BaseDbContext baseContext)
        {
            this.testService = testService;
            this.dbUsingService = dbUsingService;
            this.baseContext = baseContext;
        }

        [HttpGet("tenant-spcific-service")]
        public string GetServiceName()
        {
            return testService.GetName();
        }

        [HttpGet("tenant-spcific-db")]
        public Task<List<CommonEntity>> GetEntities()
        {
            return dbUsingService.GetEntities();
        }
    }
}
