using OobDev.Documents;
using OobDev.Documents.Markdig;
using OobDev.Documents.WkHtmlToPdf;
using OobDev.Search.Azure;
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

               .TryAddDocumentServices()
               .TryAddMarkdigServices()
               .TryAddWkHtmlToPdfServices()

               .TryAddSearchServices()
               .TryAddOllamaServices()
               .TryAddAzureServices(builder.Configuration)
               .TryAddOpenSearchServices(builder.Configuration)
               .TryAddQdrantServices(builder.Configuration)
               .TryAddSbertServices(builder.Configuration)
            ;

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
