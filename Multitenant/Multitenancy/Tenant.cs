using System.Collections.Generic;
using System.Linq;

namespace Multitenant.Multitenancy
{
    public class Tenant
    {
        public int Id { get; set; }
        public string TenantCode { get; set; }
        public string ConnectionString { get; set; }

        public virtual ICollection<ServiceMapping> TenantSpecificServices { get; set; }
        public virtual ICollection<FeatureFlag> EnabledFeatureFlags { get; set; }

        public int? AuthenticationSettingsId { get; set; }
        public virtual AuthenticationSettings AuthenticationSettings { get; set; }

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

    public class AuthenticationSettings
    {
        public int Id { get; set; }

        public bool ValidateIssuer { get; set; }
        public string ValidIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public string ValidAudience { get; set; }
        public bool ValidateLifetime { get; set; }
    }
}
