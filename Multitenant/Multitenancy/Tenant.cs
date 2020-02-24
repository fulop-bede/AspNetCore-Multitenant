namespace Multitenant.Multitenancy
{
    public class Tenant
    {
        public int Id { get; set; }
        public string TenantCode { get; set; }
        public string ConnectionString { get; set; }
    }
}
