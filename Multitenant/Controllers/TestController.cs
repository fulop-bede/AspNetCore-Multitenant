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
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly ITestService testService;
        private readonly IDbUsingService dbUsingService;
        private readonly BaseDbContext baseContext;

        public TestController(
            ILogger<TestController> logger,
            ITestService testService,
            IDbUsingService dbUsingService,
            BaseDbContext baseContext)
        {
            _logger = logger;
            this.testService = testService;
            this.dbUsingService = dbUsingService;
            this.baseContext = baseContext;
        }

        [HttpGet]
        public string Get()
        {
            return testService.Test();
        }

        [HttpGet("from-db")]
        public Task<List<CommonEntity>> GetFromDb()
        {
            return dbUsingService.GetEntities();
        }
    }
}
