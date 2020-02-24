using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Multitenant;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Multitenant.Dal;
using Multitenant.Filters;
using Multitenant.Multitenancy;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOptions();
            // This adds the required middleware to the ROOT CONTAINER and is required for multitenancy to work.
            services.AddAutofacMultitenantRequestServices();

            services.AddMemoryCache();

            // default db context containing tenant configurations
            services.AddDbContext<MasterDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
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
            // This will all go in the ROOT CONTAINER and is NOT TENANT SPECIFIC.
            builder.RegisterType<TenantService>().As<ITenantService>().SingleInstance();
            builder.RegisterType<TenantResolver>().As<ITenantResolver>().SingleInstance();
            builder.RegisterType<MyTenantIdentificationStrategy>().As<ITenantIdentificationStrategy>().SingleInstance();
            builder.RegisterType<DefaultTestService>().As<ITestService>();
            builder.RegisterType<DbUsingService>().As<IDbUsingService>();

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
            // This is the MULTITENANT PART. Set up your tenant-specific stuff here.
            var strategy = container.Resolve<ITenantIdentificationStrategy>();
            var mtc = new MultitenantContainer(strategy, container);

            mtc.ConfigureTenant("first-tenant", cb =>
            {
                IServiceCollection tenantServices = new ServiceCollection();

                tenantServices.AddDbContext<FirstTenantDbContext>();
                tenantServices.AddScoped<BaseDbContext>(provider => provider.GetService<FirstTenantDbContext>());

                cb.Populate(tenantServices);
                cb.RegisterType<FirstTenantTestService>().As<ITestService>();
            });
            mtc.ConfigureTenant("second-tenant", cb =>
            {
                IServiceCollection tenantServices = new ServiceCollection();

                tenantServices.AddDbContext<SecondTenantDbContext>();
                tenantServices.AddScoped<BaseDbContext>(provider => provider.GetService<SecondTenantDbContext>());

                cb.Populate(tenantServices);
                cb.RegisterType<SecondTenantTestService>().As<ITestService>();
            });

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
