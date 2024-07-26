using BehaviorBlackboard.Engine;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace BehaviorBlackboard;

internal class Program
{
    static void Main(string[] args)
    {
        var control = new Control(new Blackboard())
            .Add(new KnowledgeSourceHasFullName())
            .Add(new KnowledgeSourceHasEmail())
            .Snapshot(out var empty)
            .Post("first", "Matt")
            .Snapshot(out var withFirst)
            .Post("last", "Whited")
            .Snapshot(out var withLast)
            ;

        Console.WriteLine("empty"); empty.WriteOut();
        Console.WriteLine("withFirst"); withFirst.WriteOut();
        Console.WriteLine("withLast"); withLast.WriteOut();
    }
}

public static class DictionaryExtensions
{
    public static void WriteOut<K, V>(this IReadOnlyDictionary<K, V> dict)
    {
        foreach (var item in dict)
            Console.WriteLine($"\t{item.Key}= {item.Value}");
    }
}


public class KnowledgeSourceHasFullName : IKnowledgeSource
{
    public void HandleChange(IBlackboard blackboard, IEventData data)
    {
        if (data.Key.Equals("first", StringComparison.OrdinalIgnoreCase) ||
            data.Key.Equals("last", StringComparison.OrdinalIgnoreCase))
        {
            var first = blackboard["first"] as string;
            var last = blackboard["last"] as string;

            if (!string.IsNullOrWhiteSpace(first) && !string.IsNullOrWhiteSpace(last))
            {
                blackboard.Set(this, "email", $"{first}.{last}@MyDomain.com");
            }
        }
    }
}
public class KnowledgeSourceHasEmail : IKnowledgeSource
{
    public void HandleChange(IBlackboard blackboard, IEventData data)
    {
        if (data.Key.Equals("email", StringComparison.OrdinalIgnoreCase))
        {
            blackboard.Set(this, "ready", true);
        }
    }
}
