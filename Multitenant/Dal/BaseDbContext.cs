using Microsoft.EntityFrameworkCore;
using Multitenant.Multitenancy;

namespace Multitenant.Dal
{
	public abstract class BaseDbContext : DbContext
	{
		protected readonly Tenant Tenant;

		protected BaseDbContext(Tenant tenant)
		{
			Tenant = tenant;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		public virtual DbSet<CommonEntity> CommonEntities { get; set; }

		public class CommonEntity
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}
	}
}
