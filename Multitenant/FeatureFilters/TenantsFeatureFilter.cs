using Microsoft.FeatureManagement;
using Multitenant.Multitenancy.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Multitenant.FeatureFilters
{
    [FilterAlias(nameof(TenantsFeatureFilter))]
    public class TenantsFeatureFilter : IFeatureFilter
    {
        private readonly Tenant tenant;

        public TenantsFeatureFilter(Tenant tenant)
        {
            this.tenant = tenant;
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var isEnabled = tenant.EnabledFeatureFlags
                .Any(f => f.FeatureName == context.FeatureName);

            return Task.FromResult(isEnabled);
        }
    }
}