using Microsoft.FeatureManagement;
using Multitenant.Multitenancy.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Multitenant.FeatureFilters
{
    [FilterAlias("Tenants")]
    public class TenantFeatureFilter : IFeatureFilter
    {
        private readonly Tenant tenant;

        public TenantFeatureFilter(Tenant tenant)
        {
            this.tenant = tenant;
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var isEnabled = tenant.EnabledFeatureFlags
                .Any(f => f.FeatureName == FeatureFlags.RestrictedResource);

            return Task.FromResult(isEnabled);
        }
    }
}