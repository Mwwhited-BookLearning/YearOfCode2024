namespace BehaviorBlackboard.Engine;

public interface IEventData
{
    string Key { get; }
    object? Value { get; }
}
