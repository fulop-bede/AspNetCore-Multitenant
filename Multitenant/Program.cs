using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Multitenant
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host
                .CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacMultitenantServiceProviderFactory(Startup.ConfigureMultitenantContainer))
                .ConfigureWebHostDefaults(webHostBuilder => {
                    webHostBuilder
                      .UseContentRoot(Directory.GetCurrentDirectory())
                      .UseStartup<Startup>();
                })
                .Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
