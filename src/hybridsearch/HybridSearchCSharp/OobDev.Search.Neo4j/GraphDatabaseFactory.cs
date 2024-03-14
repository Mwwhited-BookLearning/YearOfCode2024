using Neo4j.Driver;

namespace OobDev.Search.Neo4j;

public class GraphDatabaseFactory
{
    public IDriver Create(string hostname = "localhost", int port = 7687) =>
         GraphDatabase.Driver($"bolt://{hostname}:{port}", AuthTokens.None);
}
