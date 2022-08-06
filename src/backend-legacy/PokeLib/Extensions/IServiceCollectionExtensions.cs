using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PokeApiNet;
using PokeLib.Cache;

namespace PokeLib.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInMemoryCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IInMemoryCache>(x => new InMemoryCache(configuration["CACHE_DIRECTORY"]));
        }

        public static void AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRedisCache>(x => new RedisCache(configuration["CACHE_DIRECTORY"], configuration["REDIS_CONNECTION_STRING"]));
        }

        public static void AddPokeApiClient(this IServiceCollection services)
        {
            services.AddSingleton(x => new PokeApiClient());
        }
    }
}
