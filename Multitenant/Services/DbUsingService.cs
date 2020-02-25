using Microsoft.EntityFrameworkCore;
using Multitenant.Dal;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Multitenant.Dal.ApplicationDbContext;

namespace Multitenant.Services
{
    public interface IDbUsingService
    {
        Task<List<CommonEntity>> GetEntities();
    }

    public class DbUsingService : IDbUsingService
    {
        private readonly ApplicationDbContext baseDbContext;

        public DbUsingService(ApplicationDbContext baseDbContext)
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
