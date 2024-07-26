namespace BehaviorBlackboard.Engine;

public interface IControl
{
    IControl Add(IKnowledgeSource source);
    IControl Post(string key, object? value);
    IControl Snapshot(out IReadOnlyDictionary<string, object> state);
}
