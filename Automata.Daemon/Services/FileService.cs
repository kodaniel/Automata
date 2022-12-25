using System.Text;
using Automata.Core.Contracts.Services;
using Automata.Daemon.JsonConverters;
using Newtonsoft.Json;

namespace Automata.Core.Services;

public class FileService : IFileService
{
    JsonSerializerSettings _serializerSettings;

    public FileService()
    {
        _serializerSettings = new JsonSerializerSettings();
        _serializerSettings.NullValueHandling = NullValueHandling.Ignore;
        _serializerSettings.Formatting = Formatting.Indented;
        _serializerSettings.Converters = new List<JsonConverter>()
        {
            new EventArgsConverter()
        };
    }

    public async Task<T?> ReadAsync<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (File.Exists(path))
        {
            var json = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<T>(json, _serializerSettings);
        }

        return default;
    }

    public async Task SaveAsync<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = JsonConvert.SerializeObject(content, _serializerSettings);
        await File.WriteAllTextAsync(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
    }

    public Task DeleteAsync(string folderPath, string fileName)
    {
        if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
        {
            return Task.Run(() => File.Delete(Path.Combine(folderPath, fileName)));
        }

        return Task.CompletedTask;
    }
}
