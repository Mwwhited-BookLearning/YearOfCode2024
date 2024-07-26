namespace BehaviorBlackboard.Engine;

public interface IBlackboard
{
    event EventHandler<(IBlackboard blackboard, IEventData eventData)> DataChanged;
    object? this[string key] { get; }
    void Set(object sender, string key, object? value);
    IReadOnlyDictionary<string, object> State { get; }
}
