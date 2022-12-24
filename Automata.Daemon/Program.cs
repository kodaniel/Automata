using Automata.Core.Contracts.Services;
using Automata.Core.Services;
using Automata.Daemon.Contracts;
using Automata.Daemon.Services;
using Automata.Daemon.Models;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Automata.Daemon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddGrpc();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddHostedService<Worker>();

            builder.Services.AddSingleton<AutomataController>();
            builder.Services.AddSingleton<PluginService>();
            builder.Services.AddSingleton<IWorkflowService, WorkflowService>();
            builder.Services.AddSingleton<IFileService, FileService>();
            builder.Services.AddSingleton<ILocalSettingsService, LocalSettingsService>();

            builder.Services.Configure<LocalSettingsOptions>(builder.Configuration.GetSection(nameof(LocalSettingsOptions)));

            //.ConfigureServices((context, services) =>
            //{
            //    services.AddGrpc();
            //    services.AddSingleton<WorkflowHandlerService>();

            //    services.AddHostedService<Worker>();

            //    services.AddSingleton<AutomataController>();
            //    services.AddSingleton<PluginService>();
            //    services.AddSingleton<IWorkflowService, WorkflowService>();
            //    services.AddSingleton<IFileService, FileService>();
            //    services.AddSingleton<ILocalSettingsService, LocalSettingsService>();

            //    services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
            //})
            var app = builder.Build();

            app.MapGrpcService<WorkflowEndpointService>();
            app.Run();
        }
    }
}