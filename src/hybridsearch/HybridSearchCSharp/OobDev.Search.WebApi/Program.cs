using OobDev.Search.Azure;
using OobDev.Search.Models;
using OobDev.Search.Ollama;
using OobDev.Search.OpenSearch;
using OobDev.Search.Qdrant;
using OobDev.Search.Sbert;

namespace OobDev.Search.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
               .AddOptions()
               .TryAddSearchServices()
               .TryAddOllamaServices()
               .TryAddAzureServices(builder.Configuration)
               .TryAddOpenSearchServices(builder.Configuration)
               .TryAddQdrantServices(builder.Configuration)
               .TryAddSbertServices(builder.Configuration)
            ;

        var sp = builder.Services.BuildServiceProvider();
        var x = sp.GetRequiredKeyedService<ISearchContent<SearchResultModel>>(SearchTypes.Semantic);
        var y = 1234;

        //    / [FromKeyedServices(SearchTypes.Semantic)] ISearchContent<SearchResultModel> semantic,
        //[FromKeyedServices(SearchTypes.Lexical)] ISearchContent<SearchResultModel> lexical,
        //[FromKeyedServices(SearchTypes.Hybrid)] ISearchContent<SearchResultModel> hybrid,
        //IEmbeddingProvider embedding,
        //ISearchContent<SearchResultModel> contentStore,
        //IGetContent< ContentReference > content,
        //IGetSummary<ContentReference> summary

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
