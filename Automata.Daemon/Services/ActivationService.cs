using Automata.Daemon.Contracts;

namespace Automata.Daemon.Services;

public class ActivationService : IHostedService
{
    private readonly ILogger<ActivationService> _logger;
    private readonly IConfiguration _configuration;
    private readonly AutomataController _automataController;
    private readonly IWorkflowService _workflowService;

    public ActivationService(ILogger<ActivationService> logger, IConfiguration configuration, AutomataController automataController, IWorkflowService workflowService)
    {
        _logger = logger;
        _configuration = configuration;
        _automataController = automataController;
        _workflowService = workflowService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Initializing daemon");

        await _automataController.InitializeAsync();
        await _workflowService.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Shutting down the daemon");

        await _workflowService.StopAsync();
        _automataController.Dispose();
    }
}