using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PokeLib.Extensions;
using PokeLib.Services;
using System;
using System.IO;

LoadEnv();
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddPokeCache(Environment.GetEnvironmentVariable("CACHE_DIRECTORY"));
builder.Services.AddSingleton<ITextFileQueryService, TextFileQueryService>();

builder.Services.AddCors();
builder.Services.AddControllers();

WebApplication app = builder.Build();

app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

app.MapControllers();

app.Run();

static void LoadEnv()
{
    string filePath = $"{Directory.GetCurrentDirectory()}\\.env";

    if (!File.Exists(filePath))
        throw new FileNotFoundException("Missing .env file.");

    foreach (string line in File.ReadAllLines(filePath))
    {
        string[] parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
            continue;

        Environment.SetEnvironmentVariable(parts[0], parts[1]);
    }
}
