using Microsoft.EntityFrameworkCore;
using Multitenant.Dal;
using Multitenant.Multitenancy.Model;
using System.Threading.Tasks;

namespace Multitenant.Multitenancy
{
    public interface ITenantService
    {
        Task<Tenant> GetTenant(string code);
    }

    public class TenantService : ITenantService
    {
        private readonly MasterDbContext masterDbContext;

        public TenantService(MasterDbContext masterDbContext)
        {
            this.masterDbContext = masterDbContext;
        }

        public Task<Tenant> GetTenant(string code)
        {
            return masterDbContext.Tenants
                .Include(t => t.TenantSpecificServices)
                .Include(t => t.EnabledFeatureFlags)
                .SingleAsync(t => t.TenantCode == code);
        }
    }
}
