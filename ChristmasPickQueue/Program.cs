using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace ChristmasPickQueue
{
    public class Program
    {
        static async Task<int> Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using IHost host = CreateHostBuilder(args).Build();

            //ExemplifyScoping(host.Services, "Scope 1");
            //ExemplifyScoping(host.Services, "Scope 2");

            await host.RunAsync();

            return 0;
        }


        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) => {}
                    //services.AddTransient<ITransientOperation, DefaultOperation>()
                    //        .AddScoped<IScopedOperation, DefaultOperation>()
                    //        .AddSingleton<ISingletonOperation, DefaultOperation>()
                    //        .AddTransient<OperationLogger>()
                );
    }
}
