using Automata.Daemon.Contracts;

namespace Automata.Daemon.Services
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly AutomataController _automataController;
        private readonly IWorkflowService _workflowService;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, AutomataController automataController, IWorkflowService workflowService)
        {
            _logger = logger;
            _configuration = configuration;
            _automataController = automataController;
            _workflowService = workflowService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _automataController.InitializeAsync();
            _workflowService.Start();
        }

        public override void Dispose()
        {
            _workflowService.Stop();

            base.Dispose();
        }
    }
}