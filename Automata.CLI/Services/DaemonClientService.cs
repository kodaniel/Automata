using AutoMapper;
using Automata.Runtime.Models;
using Automata.Workflow;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Automata.CLI.Services;

internal class DaemonClientService
{
    private readonly ILogger<DaemonClientService> _logger;
    private readonly WorkflowHandler.WorkflowHandlerClient _workflowClient;
    private readonly IMapper _mapper;

    public DaemonClientService(ILogger<DaemonClientService> logger, WorkflowHandler.WorkflowHandlerClient workflowClient, IMapper mapper)
    {
        _logger = logger;
        _workflowClient = workflowClient;
        _mapper = mapper;
    }

    public async IAsyncEnumerable<WorkflowArgs> ListWorkflowsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var asyncStreamingResult = _workflowClient.ListWorkflows(new Google.Protobuf.WellKnownTypes.Empty(), cancellationToken: cancellationToken);
        await foreach (var workflow in asyncStreamingResult.ResponseStream.ReadAllAsync())
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return _mapper.Map<WorkflowArgs>(workflow);
        }
    }

    public Task<bool> StartAsync(string id, CancellationToken cancellationToken = default)
    {
        var request = new WorkflowRequest { Id = id };
        var response = _workflowClient.Start(request);

        return Task.FromResult(response.Success);
    }

    public Task<bool> StopAsync(string id, CancellationToken cancellationToken = default)
    {
        var request = new WorkflowRequest { Id = id };
        var response = _workflowClient.Stop(request);

        return Task.FromResult(response.Success);
    }
}
