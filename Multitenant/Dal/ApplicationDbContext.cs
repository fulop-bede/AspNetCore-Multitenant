using Microsoft.EntityFrameworkCore;
using Multitenant.Multitenancy;
using Multitenant.Multitenancy.Model;

namespace Multitenant.Dal
{
    public class ApplicationDbContext : DbContext
    {
        protected readonly Tenant Tenant;

        public ApplicationDbContext(Tenant tenant)
        {
            Tenant = tenant;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Tenant.ConnectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure();
                sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name);
            });

            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<TestEntity> TestEntities { get; set; }
        public virtual DbSet<CommonEntity> CommonEntities { get; set; }

        public class TestEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string FirstTenantSpecificProperty { get; set; }
        }

        public class CommonEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
