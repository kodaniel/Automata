using Automata.Core.Contracts.Workflow;
using Newtonsoft.Json;

namespace Automata.Daemon.JsonConverters;

class EventArgsConverter : JsonConverter<BaseEventArgs>
{
    public override BaseEventArgs? ReadJson(JsonReader reader, Type objectType, BaseEventArgs? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return (BaseEventArgs?)Activator.CreateInstance(objectType);
    }

    public override void WriteJson(JsonWriter writer, BaseEventArgs? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            return;
        }

        writer.WriteStartObject();
        writer.WritePropertyName("Type");
        writer.WriteValue(value.GetType().AssemblyQualifiedName);
        serializer.Serialize(writer, value);
        writer.WriteEndObject();
    }
}
