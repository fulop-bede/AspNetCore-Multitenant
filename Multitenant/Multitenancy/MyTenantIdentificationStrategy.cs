using Autofac.Multitenant;
using Microsoft.AspNetCore.Http;

namespace Multitenant.Multitenancy
{
    public class MyTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public MyTenantIdentificationStrategy(
          IHttpContextAccessor httpContextAccessor
        )
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool TryIdentifyTenant(out object tenantId)
        {
            tenantId = null;
            var context = httpContextAccessor.HttpContext;
            if (context == null)
                return false;

            tenantId = (string)context?.Request?.Headers["X-Tenant-Id"];
            return (tenantId != null || tenantId == (object)"");
        }
    }
}
