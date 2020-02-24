using Microsoft.EntityFrameworkCore;
using Multitenant.Multitenancy;

namespace Multitenant.Dal
{
    public class SecondTenantDbContext : BaseDbContext
    {
        public SecondTenantDbContext(Tenant tenant)
            : base(tenant)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Tenant.ConnectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure();
                sqlOptions.MigrationsAssembly(typeof(SecondTenantDbContext).Assembly.GetName().Name);
            });

            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<TestEntity> TestEntities { get; set; }

        public class TestEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string SecondTenantSpecificProperty { get; set; }
        }
    }
}
