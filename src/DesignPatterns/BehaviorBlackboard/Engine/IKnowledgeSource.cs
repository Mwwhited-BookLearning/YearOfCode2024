namespace BehaviorBlackboard.Engine;

public interface IKnowledgeSource
{
    void HandleChange( IBlackboard blackboard, IEventData data);
}
