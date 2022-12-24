using AutoMapper;
using Automata.Core.Helpers;
using Automata.Daemon.Contracts;
using Automata.Daemon.Models;
using Automata.Workflow;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Text;

namespace Automata.Daemon.Services;

internal class WorkflowEndpointService : WorkflowHandler.WorkflowHandlerBase
{
    private readonly ILogger<WorkflowEndpointService> _logger;
    private readonly IWorkflowService _workflowService;
    private readonly IMapper _mapper;

    public WorkflowEndpointService(ILogger<WorkflowEndpointService> logger, IWorkflowService workflowService, IMapper mapper)
    {
        _logger = logger;
        _workflowService = workflowService;
        _mapper = mapper;
    }

    public override Task<WorkflowResponse> Start(WorkflowRequest request, ServerCallContext context)
    {
        var success = false;
        var data = new WorkflowMessage();

        var workflow = _workflowService.Get(request.Id);
        if (workflow is null)
        {
            _logger.LogError("Invalid workflow id.");
        }
        else
        {
            workflow.IsEnabled = true;
            success = true;
            data = _mapper.Map<WorkflowMessage>(workflow);
            _logger.LogInformation("Workflow started.");
        }

        return Task.FromResult(new WorkflowResponse { Success = success, Data = data });
    }

    public override Task<WorkflowResponse> Stop(WorkflowRequest request, ServerCallContext context)
    {
        var success = false;
        var data = new WorkflowMessage();

        var workflow = _workflowService.Get(request.Id);
        if (workflow is null)
        {
            _logger.LogError("Invalid workflow id.");
        }
        else
        {
            workflow.IsEnabled = false;
            success = true;
            data = _mapper.Map<WorkflowMessage>(workflow);
            _logger.LogInformation("Workflow stopped.");
        }

        return Task.FromResult(new WorkflowResponse { Success = success, Data = data });
    }

    public override async Task ListWorkflows(Empty request, IServerStreamWriter<WorkflowMessage> responseStream, ServerCallContext context)
    {
        var workflows = _workflowService.Workflows;
        foreach (var workflow in workflows)
        {
            var workflowDto = _mapper.Map<WorkflowMessage>(workflow);
            await responseStream.WriteAsync(workflowDto, context.CancellationToken);
        }
    }
}
