using AutoMapper;
using Automata.Core.Contracts.Workflow;
using Automata.Core.Helpers;
using Automata.Core.Models;
using Automata.Daemon.Models;
using Automata.Workflow;
using System.Text;

namespace Automata.Daemon.Profiles;

class WorkflowProfile : Profile
{
    public WorkflowProfile()
    {
        CreateMap<WorkflowArgs, WorkflowMessage>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsEnabled ? WorkflowStatus.Enabled : WorkflowStatus.Disabled))
            .AfterMap(async (src, dest, ctx) =>
            {
                var eventDto = ctx.Mapper.Map<EventDto>(src.Event);
                dest.EventBody = await SerializeBase64String(eventDto);
            });

        CreateMap<BaseEventArgs, EventDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.GetType().AssemblyQualifiedName))
            .AfterMap((src, dest, ctx) =>
            {
                dest.Fields = new();
                var properties = src.GetType().GetProperties().Where(p => typeof(FieldArgs).IsAssignableFrom(p.PropertyType));
                foreach (var property in properties)
                {
                    var field = (FieldArgs)property.GetValue(src, null);
                    dest.Fields[property.Name] = ctx.Mapper.Map<FieldDto>(field);
                }
            });

        CreateMap<FieldArgs, FieldDto>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.Expression, opt => opt.MapFrom(src => src.Expression))
            .ForMember(dest => dest.IsExpression, opt => opt.MapFrom(src => src.IsExpression))
            .ForMember(dest => dest.ContextId, opt => opt.MapFrom(src => src.ContextId));
    }

    private static async Task<string> SerializeBase64String(object instance)
    {
        var json = await Json.StringifyAsync(instance);
        var bytes = Encoding.UTF8.GetBytes(json);
        return Convert.ToBase64String(bytes);
    }
}

class EventDto
{
    public string Type { get; set; }
    public Dictionary<string, FieldDto> Fields { get; set; }
}

class FieldDto
{
    public string Value { get; set; }
    public string Expression { get; set; }
    public string ContextId { get; set; }
    public bool IsExpression { get; set; }
}