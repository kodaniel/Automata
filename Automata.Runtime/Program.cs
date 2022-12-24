using Automata.Core.Contracts.Services;
using Automata.Core.Services;
using Automata.Models;
using Automata.Runtime.CLI;
using Automata.Runtime.Contracts;
using Automata.Runtime.Services;
using Automata.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Automata.Runtime;

public class Program
{
    private async static Task<int> Main(string[] args)
    {
        var Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "\\appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var builder = new HostBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ActivationService>();

                services.AddSingleton<AutomataController>();
                services.AddSingleton<PluginService>();
                services.AddSingleton<IWorkflowService, WorkflowService>();
                services.AddSingleton<IFileService, FileService>();
                services.AddSingleton<ILocalSettingsService, LocalSettingsService>();

                services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
            });

        try
        {
            DebugHelper.HandleDebugSwitch(ref args);

            return await builder.RunCommandLineApplicationAsync<AutomataCmd>(args);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 1;
        }
    }
}