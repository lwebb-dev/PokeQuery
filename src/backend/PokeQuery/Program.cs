using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using PokeQuery.Services;

namespace PokeQuery
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string PokeQueryOrigins = "_pokeQueryOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: PokeQueryOrigins,
                                  policy =>
                                  {
                                      policy
                                        .WithOrigins(new string[] { "http://localhost:3000", "http://localhost:5173" }) 
                                        .WithHeaders(HeaderNames.ContentType);
                                  });
            });

            builder.Services.AddSingleton<IRedisService, RedisService>();
            builder.Services.AddControllers();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors(PokeQueryOrigins);

            app.Run();
        }
    }
}