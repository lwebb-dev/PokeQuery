using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PokeLib.Extensions;
using PokeLib.Services;
using PokeLib.Utilities;
using System;

EnvironmentVarUtility.LoadEnvironmentVariablesFromDotEnv();
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string cacheDirectory = Environment.GetEnvironmentVariable("CACHE_DIRECTORY");
string redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
builder.Services.AddInMemoryCache(cacheDirectory);
builder.Services.AddRedisCache(cacheDirectory, redisConnectionString);
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
