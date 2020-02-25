using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Multitenant;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using Multitenant.Dal;
using Multitenant.FeatureFilters;
using Multitenant.Filters;
using Multitenant.Multitenancy;
using Multitenant.Multitenancy.Model;
using Multitenant.Services;

namespace Multitenant
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOptions();

            // This adds the required middleware to the ROOT CONTAINER and is required for multitenancy to work.
            services.AddAutofacMultitenantRequestServices();

            services.AddMemoryCache();

            services
                .AddFeatureManagement()
                .AddFeatureFilter<TenantsFeatureFilter>();

            // default db context containing tenant configurations
            services.AddDbContext<MasterDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MasterDbConnection"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WebApi"
                });
                c.OperationFilter<RequiredHeaderOperationFilter>();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<TenantService>().As<ITenantService>().SingleInstance();
            builder.RegisterType<TenantResolver>().As<ITenantResolver>().SingleInstance();
            builder.RegisterType<MyTenantIdentificationStrategy>().As<ITenantIdentificationStrategy>().SingleInstance();
            builder.RegisterType<DbUsingService>().As<IDbUsingService>();
            builder.RegisterType<FirstTenantTestService>().Keyed<ITestService>(nameof(FirstTenantTestService));
            builder.RegisterType<SecondTenantTestService>().Keyed<ITestService>(nameof(SecondTenantTestService));

            IServiceCollection tenantServices = new ServiceCollection();

            tenantServices.AddDbContext<ApplicationDbContext>();

            builder.Populate(tenantServices);

            builder.Register<Tenant>(container =>
            {
                ITenantIdentificationStrategy strategy = container.Resolve<ITenantIdentificationStrategy>();
                strategy.TryIdentifyTenant(out object id);
                if (id != null)
                {
                    if (container.IsRegistered(typeof(ITenantResolver)))
                    {
                        var tenantResolver = container.Resolve<ITenantResolver>();
                        return tenantResolver.ResolveAsync(id).Result;
                    }
                }
                return new Tenant();
            }).InstancePerLifetimeScope();
        }

        public static MultitenantContainer ConfigureMultitenantContainer(IContainer container)
        {
            var strategy = container.Resolve<ITenantIdentificationStrategy>();
            var mtc = new MultitenantContainer(strategy, container);
            return mtc;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test Web API");
                });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
