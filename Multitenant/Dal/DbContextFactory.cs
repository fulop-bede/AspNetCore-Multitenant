using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Multitenant.Multitenancy.Model;
using System.IO;

namespace Multitenant.Dal
{
	public class FirstTenantDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
											   .SetBasePath(Directory.GetCurrentDirectory())
											   .AddJsonFile("appsettings.json")
											   .AddJsonFile("appsettings.Development.json")
											   .Build();

			Tenant tenant = new Tenant();
			var connectionString = configuration.GetValue<string>("ConnectionStrings:ApplicationDbConnection");
			tenant.ConnectionString = connectionString;

			return new ApplicationDbContext(tenant);
		}
	}

	public class MasterDbContextFactory : IDesignTimeDbContextFactory<MasterDbContext>
	{
		public MasterDbContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
											   .SetBasePath(Directory.GetCurrentDirectory())
											   .AddJsonFile("appsettings.json")
											   .AddJsonFile("appsettings.Development.json")
											   .Build();

			var connectionString = configuration.GetValue<string>("ConnectionStrings:MasterDbConnection");
			return new MasterDbContext(connectionString);
		}
	}
}
