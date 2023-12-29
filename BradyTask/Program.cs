using BradyTask.BusinessLogic;
using BradyTask.BusinessLogic.Contracts;
using BradyTask.BusinessLogic.Models.Helpers;
using BradyTask.BusinessLogic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BradyTask;
class Program
{
    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder();
        var config = BuildConfig(builder);

        var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddScoped<IFileProcessingService, FileProcessingService>();
            services.AddScoped<IFilesCheckerService, FilesCheckerService>();
            services.Configure<PathConfiguration>(config.GetSection("PathConfiguration"));
        }).Build();

        var svc = ActivatorUtilities.CreateInstance<FileProcessingService>(host.Services);
        svc.Process();
    }

    static IConfiguration BuildConfig(IConfigurationBuilder builder)
    {
        return builder.SetBasePath(Directory.GetCurrentDirectory() + "\\Configurations")
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables().Build();
    }
}