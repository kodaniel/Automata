using Automata.Core.Contracts.EventAggregator;
using Microsoft.Extensions.DependencyInjection;

namespace Automata.Core.Services;
public class CoreServices
{
    private readonly IServiceProvider _serviceProvider;

    private CoreServices(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    //public ILogger Logger => _serviceProvider.GetRequiredService<ILogger>();

    public IMessenger Messenger => _serviceProvider.GetRequiredService<IMessenger>();


    private static CoreServices _instance;
    public static CoreServices Instance => _instance ??
        throw new InvalidOperationException("Call the Install() method to use this class.");

    public static void Install(IServiceProvider serviceProvider)
    {
        _instance = new CoreServices(serviceProvider);
    }
}