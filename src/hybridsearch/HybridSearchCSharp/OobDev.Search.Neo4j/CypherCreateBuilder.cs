using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace OobDev.Search.Neo4j;

public class CypherCreateBuilder
{
    public bool IsNodeProperty(Type type) =>
        type.IsEnum ||
        new[] {
            typeof(string),

            typeof(DateTime),typeof(DateTime?),
            typeof(DateTimeOffset),typeof(DateTimeOffset?),
            typeof(TimeSpan),typeof(TimeSpan?),
            typeof(TimeOnly),typeof(TimeOnly?),
            typeof(DateOnly),typeof(DateOnly?),

            typeof(Guid),typeof(Guid?),
            typeof(char),typeof(char?),

            typeof(int),typeof(int?),
            typeof(uint),typeof(uint?),
            typeof(long),typeof(long?),
            typeof(ulong),typeof(int?),
            typeof(short),typeof(short?),
            typeof(ushort),typeof(ushort?),
            typeof(byte),typeof(byte?),
            typeof(sbyte),typeof(sbyte?),
            typeof(float),typeof(float?),
            typeof(double),typeof(double?),
            typeof(decimal),typeof(decimal?),
        }.Contains(type) || type.IsArray && type.GetElementType() != null && IsNodeProperty(type.GetElementType());

    public bool IsInitOnly(PropertyInfo type) =>
        type.CanWrite &&
        (type.SetMethod?.ReturnParameter.GetRequiredCustomModifiers()
            .Any(a => a.Name == "IsExternalInit") ?? false);

    public object? ToValue(PropertyInfo property, object instance)
    {
        var value = property.GetValue(instance, null);
        if (value != null)
        {
            if (value is DateTime dt)
                return dt.Ticks;
            else if (value is DateTimeOffset dto)
                return dto.UtcTicks;
            else if (value is TimeSpan ts)
                return ts.Ticks;
            else if (value is TimeOnly to)
                return to.Ticks;
            else if (value is DateOnly @do)
                return @do.ToString("yyyyMMdd");

            else if (value is Guid uuid)
                return uuid.ToString();

            else if (value is byte[] byteArray)
                return Convert.ToBase64String(byteArray);
        }
        return value;
    }

    public string GetName(Type type, string? prefix = null) =>
        (prefix != null ? prefix + "_" : "") + type.Name;

    public string BuildUpsert(Type type, string? prefix = null)
    {
        var name = type.Name;
        var sName = GetName(type, prefix);
        var propPrefix = prefix != null ? prefix + "_" : "";
        var properties = type.GetProperties();
        var keyProperties = properties.Where(p => p.GetCustomAttribute<KeyAttribute>() != null).ToArray();
        var otherProperties = properties.Except(keyProperties)
            .Where(p => IsNodeProperty(p.PropertyType))
            .ToArray();

        //MERGE(u: Person { ID: 123})
        //ON CREATE SET u.name = 'New User', u.created_at = timestamp()
        //ON MATCH SET u.last_login = timestamp()
        //RETURN u

        var sb = new StringBuilder();
        var keys = from prop in keyProperties
                   select $"{prop.Name}: ${propPrefix}{prop.Name}";
        sb.AppendLine($"MERGE ({sName}:{name} {{ {string.Join(", ", keys)} }})");

        sb.AppendLine($"ON CREATE SET ");
        foreach (var prop in otherProperties)
            sb.AppendLine($"\t{sName}.{prop.Name}=${propPrefix}{prop.Name}, ");
        sb.AppendLine($"\t{sName}.__last_action='CREATED', ");
        sb.AppendLine($"\t{sName}.__modified_on=timestamp(), ");
        sb.AppendLine($"\t{sName}.__created_on=timestamp()");

        var onUpdate = from prop in otherProperties
                       where prop.CanWrite
                       where prop.SetMethod != null
                       where !IsInitOnly(prop)
                       select prop;

        sb.AppendLine($"ON MATCH SET ");
        foreach (var prop in onUpdate)
            sb.AppendLine($"\t{sName}.{prop.Name}=${propPrefix}{prop.Name}, ");
        sb.AppendLine($"\t{sName}.__last_action='UPATED', ");
        sb.AppendLine($"\t{sName}.__modified_on=timestamp()");

        return sb.ToString();
    }

    public record ReferenceState
    {
        public required object Instance { get; init; }
        public required string Name { get; init; }
        public required string Script { get; init; }
    }

    public IDictionary<string, object> GetParameters(Type type, object instance, string? prefix = null) =>
        (from prop in type.GetProperties()
         where IsNodeProperty(prop.PropertyType)
         select new
         {
             prop.Name,
             Value = ToValue(prop, instance)
         }).ToDictionary(k => $"{(prefix != null ? prefix + "_" : "")}{k.Name}", v => v.Value);

    public string BuildReturns(params string[] names) => $"RETURN {string.Join(", ", names)}";

    public string BuildRelationship(
        string fromName, string toName, string type, string prefix
    ) =>
    new StringBuilder()
        .AppendLine($"MERGE ({fromName})-[{prefix}:{type}]->({toName})")

        .AppendLine($"ON CREATE SET")
        .AppendLine($"\t{prefix}.__last_action='CREATE', ")
        .AppendLine($"\t{prefix}.__modified_on=timestamp() ,")
        .AppendLine($"\t{prefix}.__created_on=timestamp()")

        .AppendLine($"ON MATCH SET")
        .AppendLine($"\t{prefix}.__last_action='UPATED', ")
        .AppendLine($"\t{prefix}.__modified_on=timestamp()")

        //TODO: could be handing to have a matching property set here
        .ToString();

    //public string BuildUniqueIndex(Type model)
    //{
    //// CREATE CONSTRAINT ON (person:Person) ASSERT person.email IS UNIQUE;
    //}

}
