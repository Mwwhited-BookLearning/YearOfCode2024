
namespace BehaviorBlackboard.Engine;

public class Control : IControl
{
    private readonly IBlackboard _blackboard;
    private readonly List<IKnowledgeSource> _knowledgeSources = [];
    public Control(IBlackboard blackboard) => _blackboard = blackboard;

    public IControl Add(IKnowledgeSource knowledgeSource)
    {
        _blackboard.DataChanged += (s, e) =>
        {
            if (s == knowledgeSource) return;

            knowledgeSource.HandleChange(e.blackboard, e.eventData);
        };
        _knowledgeSources.Add(knowledgeSource);
        return this;
    }

    public IControl Post(string key, object? value)
    {
        _blackboard.Set(this, key, value);
        return this;
    }

    public IControl Snapshot(out IReadOnlyDictionary<string, object> state)
    {
        state = _blackboard.State;
        return this;
    }
}
