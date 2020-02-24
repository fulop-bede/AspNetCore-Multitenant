using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Multitenant.Multitenancy;
using System.IO;

namespace Multitenant.Dal
{
	public class FirstTenantDbContextFactory : IDesignTimeDbContextFactory<FirstTenantDbContext>
	{
		public FirstTenantDbContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
											   .SetBasePath(Directory.GetCurrentDirectory())
											   .AddJsonFile("appsettings.json")
											   .AddJsonFile("appsettings.Development.json")
											   .Build();

			Tenant tenant = new Tenant();
			configuration.GetSection("Multitenancy:Tenants:0").Bind(tenant);

			return new FirstTenantDbContext(tenant);
		}
	}

	public class SecondTenantDbContextFactory : IDesignTimeDbContextFactory<SecondTenantDbContext>
	{
		public SecondTenantDbContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
											   .SetBasePath(Directory.GetCurrentDirectory())
											   .AddJsonFile("appsettings.json")
											   .AddJsonFile("appsettings.Development.json")
											   .Build();

			Tenant tenant = new Tenant();
			configuration.GetSection("Multitenancy:Tenants:1").Bind(tenant);

			return new SecondTenantDbContext(tenant);
		}
	}
}
