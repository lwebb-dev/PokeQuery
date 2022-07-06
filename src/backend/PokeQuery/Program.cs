using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PokeLib.Extensions;
using PokeLib.Services;
using PokeLib.Utilities;

EnvironmentVarUtility.LoadEnvironmentVariablesFromDotEnv();
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.WebHost.UseUrls(configuration["API_BASE_URI"]);

builder.Services.AddInMemoryCache(configuration);
builder.Services.AddRedisCache(configuration);
builder.Services.AddSingleton<IInMemoryCacheQueryService, InMemoryCacheQueryService>();
builder.Services.AddSingleton<IRedisQueryService, RedisQueryService>();

builder.Services.AddCors();
builder.Services.AddControllers();

WebApplication app = builder.Build();

app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

app.MapControllers();

app.Run();
