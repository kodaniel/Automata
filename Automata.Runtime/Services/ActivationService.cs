using Automata.Runtime.Contracts;
using Microsoft.Extensions.Hosting;

namespace Automata.Runtime.Services;

internal class ActivationService : IHostedService
{
    private readonly AutomataController _automataController;
    private readonly IWorkflowService _workflowService;

    public ActivationService(AutomataController automataController, IWorkflowService workflowService)
    {
        _automataController = automataController;
        _workflowService = workflowService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _automataController.InitializeAsync();
        _workflowService.Start();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _workflowService.Stop();

        return Task.CompletedTask;
    }
}
