using Automata.CLI.Commands;
using Automata.CLI.Services;
using Automata.Workflow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "\\appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

var builder = new HostBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<DaemonClientService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddGrpcClient<WorkflowHandler.WorkflowHandlerClient>(options =>
        {
            options.Address = new Uri("https://localhost:5001");
        });
    });

try
{
    DebugHelper.HandleDebugSwitch(ref args);

    return await builder.RunCommandLineApplicationAsync<AutomataCmd>(args);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    return 1;
}
