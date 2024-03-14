using OobDev.Search.Azure;
using OobDev.Search.Ollama;
using OobDev.Search.OpenSearch;
using OobDev.Search.Sbert;

namespace OobDev.Search.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
               .TryAddSearchServices()
               .TryAddOllamaServices()
               .TryAddAzureServices()
               .TryAddOpenSearchServices()
               .TryAddSbertServices()
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
