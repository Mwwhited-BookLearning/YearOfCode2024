
using System.Collections.Concurrent;

namespace BehaviorBlackboard.Engine;

public class Blackboard : IBlackboard
{
    private readonly ConcurrentDictionary<string, object> _data = new(StringComparer.OrdinalIgnoreCase);

    public void Set(object sender, string key, object? value)
    {
        var has = _data.TryGetValue(key, out var current);

        var changed = false;

        if (has)
        {
            if (value == null && current != null)
            {
                //remove
                _data.TryRemove(key, out var _);
                changed = true;
            }
            else if (value != null && !object.Equals(value, current))
            {
                //replace
                changed = true;
                _data.TryUpdate(key, value, current);
            }
        }
        else if (value != null)
        {
            //add 
            changed = true;
            _data.TryAdd(key, value);
        }

        if (changed)
            DataChanged?.Invoke(sender, (this, new EventData() { Key = key, Value = value }));
    }

    public object? this[string key]
    {
        get => _data.TryGetValue(key, out var value) ? value : null;
    }
    public IReadOnlyDictionary<string, object> State => new Dictionary<string, object>(_data).AsReadOnly();

    public event EventHandler<(IBlackboard, IEventData)> DataChanged;
}
