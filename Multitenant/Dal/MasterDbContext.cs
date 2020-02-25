using Microsoft.EntityFrameworkCore;
using Multitenant.Multitenancy.Model;

namespace Multitenant.Dal
{
    public class MasterDbContext : DbContext
    {
        private readonly string connectionString;

        public MasterDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                    sqlOptions.MigrationsAssembly(typeof(MasterDbContext).Assembly.GetName().Name);
                });
            }

            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Tenant> Tenants { get; set; }
        public virtual DbSet<ServiceMapping> ServiceMappings { get; set; }
        public virtual DbSet<FeatureFlag> FeatureFlags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>()
                .HasMany(t => t.TenantSpecificServices)
                .WithOne(s => s.Tenant)
                .HasForeignKey(s => s.TenantId);
            modelBuilder.Entity<Tenant>()
                .HasMany(t => t.EnabledFeatureFlags)
                .WithOne(s => s.Tenant)
                .HasForeignKey(s => s.TenantId);
        }
    }
}
