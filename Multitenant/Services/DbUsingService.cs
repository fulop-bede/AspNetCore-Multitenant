using Microsoft.EntityFrameworkCore;
using Multitenant.Dal;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Multitenant.Dal.BaseDbContext;

namespace Multitenant.Services
{
    public interface IDbUsingService
    {
        Task<List<CommonEntity>> GetEntities();
    }

    public class DbUsingService : IDbUsingService
    {
        private readonly BaseDbContext baseDbContext;

        public DbUsingService(BaseDbContext baseDbContext)
        {
            this.baseDbContext = baseDbContext;
        }

        public Task<List<CommonEntity>> GetEntities()
        {
            return baseDbContext.CommonEntities
                .ToListAsync();
        }
    }
}
