using Microsoft.EntityFrameworkCore;
using Multitenant.Multitenancy;

namespace Multitenant.Dal
{
    public class MasterDbContext : DbContext
    {
        public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Tenant> Tenants { get; set; }
    }
}
