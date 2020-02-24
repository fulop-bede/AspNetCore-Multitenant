using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Multitenant.Multitenancy
{
    public interface ITenantResolver
    {
        Task<Tenant> ResolveAsync(object tenantId);
    }

    public class TenantResolver : ITenantResolver
    {
        private readonly IMemoryCache memoryCache;
        private readonly ITenantService tenantService;

        public TenantResolver(
          IMemoryCache memoryCache,
          ITenantService tenantService
        )
        {
            this.memoryCache = memoryCache;
            this.tenantService = tenantService;
        }

        public async Task<Tenant> ResolveAsync(object tenantId)
        {
            Tenant tenant;
            var tId = (string)tenantId;
            if (memoryCache.TryGetValue(tId, out object cached))
            {
                tenant = (Tenant)cached;
            }
            else
            {
                tenant = await tenantService.GetTenant(tId);
                memoryCache.Set(tId, tenant);
            }

            return tenant;
        }
    }
}
