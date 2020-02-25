using Autofac.Features.Indexed;
using Multitenant.Multitenancy;

namespace Multitenant.Extensions
{
    public static class IIndexExtensions
    {
        public static T GetImplementation<T>(this IIndex<string, T> index, Tenant tenant)
        {
            return index[tenant.GetImplementationTypeName(typeof(T).Name)];
        }
    }
}
