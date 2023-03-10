using Automata.Core.Contracts.Services;
using Automata.Daemon.Models;
using Automata.Daemon.Contracts;
using Microsoft.Extensions.Options;
using Automata.Core.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Automata.Daemon.Services;

public class LocalSettingsService : ILocalSettingsService
{
    private const string _defaultApplicationDataFolder = "Automata/ApplicationData";
    private const string _defaultLocalSettingsFile = "LocalSettings.json";

    private readonly IFileService _fileService;
    private readonly LocalSettingsOptions _options;

    private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private readonly string _applicationDataFolder;
    private readonly string _localsettingsFile;

    private IDictionary<string, object?> _settings;

    private bool _isInitialized;

    public LocalSettingsService(IFileService fileService, IOptions<LocalSettingsOptions> options)
    {
        _fileService = fileService;
        _options = options.Value;

        _applicationDataFolder = Path.Combine(_localApplicationData, _options.ApplicationDataFolder ?? _defaultApplicationDataFolder);
        _localsettingsFile = _options.LocalSettingsFile ?? _defaultLocalSettingsFile;

        _settings = new Dictionary<string, object?>();
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            _settings = await _fileService.ReadAsync<IDictionary<string, object?>>(_applicationDataFolder, _localsettingsFile) ?? new Dictionary<string, object?>();

            _isInitialized = true;
        }
    }

    public async Task<T?> ReadSettingAsync<T>(string key)
    {
        await InitializeAsync();

        if (_settings != null && _settings.TryGetValue(key, out var obj) && obj is not null)
        {
            var jobj = (JObject)obj;
            var o = jobj.ToObject<T>();
            return await Json.ToObjectAsync<T>(obj.ToString());
        }

        return default;
    }

    public async Task SaveSettingAsync<T>(string key, T value)
    {
        await InitializeAsync();

        _settings[key] = value;

        await _fileService.SaveAsync(_applicationDataFolder, _localsettingsFile, _settings);
    }
}
