#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
ENV ASPNETCORE_HTTPS_PORTS=8081
ENV ASPNETCORE_HTTP_PORTS=8080
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OobDev.Search.WebApi/OobDev.Search.WebApi.csproj", "OobDev.Search.WebApi/"]
COPY ["OobDev.Documents.Markdig/OobDev.Documents.Markdig.csproj", "OobDev.Documents.Markdig/"]
COPY ["OobDev.Documents.Abstractions/OobDev.Documents.Abstractions.csproj", "OobDev.Documents.Abstractions/"]
COPY ["OobDev.Documents.WkHtmlToPdf/OobDev.Documents.WkHtmlToPdf.csproj", "OobDev.Documents.WkHtmlToPdf/"]
COPY ["OobDev.Documents/OobDev.Documents.csproj", "OobDev.Documents/"]
COPY ["OobDev.Search.Azure/OobDev.Search.Azure.csproj", "OobDev.Search.Azure/"]
COPY ["OobDev.Search.Abstractions/OobDev.Search.Abstractions.csproj", "OobDev.Search.Abstractions/"]
COPY ["OobDev.Search.Neo4j/OobDev.Search.Neo4j.csproj", "OobDev.Search.Neo4j/"]
COPY ["OobDev.Search.Ollama/OobDev.Search.Ollama.csproj", "OobDev.Search.Ollama/"]
COPY ["OobDev.Search.OpenSearch/OobDev.Search.OpenSearch.csproj", "OobDev.Search.OpenSearch/"]
COPY ["OobDev.Search.Qdrant/OobDev.Search.Qdrant.csproj", "OobDev.Search.Qdrant/"]
COPY ["OobDev.Search/OobDev.Search.csproj", "OobDev.Search/"]
COPY ["OobDev.Search.Sbert/OobDev.Search.Sbert.csproj", "OobDev.Search.Sbert/"]
RUN dotnet restore "./OobDev.Search.WebApi/OobDev.Search.WebApi.csproj"
COPY . .
WORKDIR "/src/OobDev.Search.WebApi"
RUN dotnet build "./OobDev.Search.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OobDev.Search.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OobDev.Search.WebApi.dll"]