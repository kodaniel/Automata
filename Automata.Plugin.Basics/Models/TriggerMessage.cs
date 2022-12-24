using Automata.Core.Contracts.EventAggregator;

namespace Automata.Plugin.Basics.Models;
public record struct TriggerMessage(string Id, string? Message) : IMessage;
