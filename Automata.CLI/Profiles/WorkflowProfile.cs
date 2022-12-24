using AutoMapper;
using Automata.Core.Contracts.Workflow;
using Automata.Core.Helpers;
using Automata.Runtime.Models;
using Automata.Workflow;
using System.Text;

namespace Automata.CLI.Profiles;

class WorkflowProfile : Profile
{
	public WorkflowProfile()
	{
        CreateMap<WorkflowMessage, WorkflowArgs>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => new Guid(src.Id)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.Status == WorkflowStatus.Enabled))
            .AfterMap(async (src, dest, context) =>
            {
                var eventDto = await DeserializeBase64String<EventDto>(src.EventBody);
                dest.Event = context.Mapper.Map<BaseEventArgs>(eventDto);
            });

        CreateMap<EventDto, BaseEventArgs>()
            .ForMember(dest => dest.UniqueId, opt => opt.Ignore())
            .ConvertUsing(new EventTypeConverter());
	}

    private static async Task<T> DeserializeBase64String<T>(string base64string)
    {
        var bytes = Convert.FromBase64String(base64string);
        var json = Encoding.UTF8.GetString(bytes);
        return await Json.ToObjectAsync<T>(json);
    }

    public class EventTypeConverter : ITypeConverter<EventDto, BaseEventArgs>
    {
        BaseEventArgs ITypeConverter<EventDto, BaseEventArgs>.Convert(EventDto source, BaseEventArgs destination, ResolutionContext context)
        {
            var type = Type.GetType(source.Type);
            return (BaseEventArgs)Activator.CreateInstance(type)!;
        }
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