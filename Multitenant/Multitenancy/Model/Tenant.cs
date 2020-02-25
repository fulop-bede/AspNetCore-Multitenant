using System.Collections.Generic;
using System.Linq;

namespace Multitenant.Multitenancy.Model
{
    public class Tenant
    {
        public int Id { get; set; }
        public string TenantCode { get; set; }
        public string ConnectionString { get; set; }

        public virtual ICollection<ServiceMapping> TenantSpecificServices { get; set; }
        public virtual ICollection<FeatureFlag> EnabledFeatureFlags { get; set; }

        public string GetImplementationTypeName(string type)
        {
            return TenantSpecificServices
                .Single(s => s.Type == type).Implementation;
        }
    }

    public class ServiceMapping
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        public string Type { get; set; }
        public string Implementation { get; set; }
    }

    public class FeatureFlag
    {
        public int Id { get; set; }

        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        public string FeatureName { get; set; }
    }
}
