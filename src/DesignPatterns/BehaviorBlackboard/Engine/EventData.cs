namespace BehaviorBlackboard.Engine;

public record EventData : IEventData
{
    public required string Key { get; set; }

    public required object? Value { get; set; }
}
