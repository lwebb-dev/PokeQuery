using Microsoft.Extensions.DependencyInjection;
using PokeLib.Cache;

namespace PokeLib.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInMemoryCache(this IServiceCollection services, string cacheDirectory)
        {
            services.AddSingleton<IInMemoryCache>(x => new InMemoryCache(cacheDirectory));
        }

        public static void AddRedisCache(this IServiceCollection services, string cacheDirectory, string redisConnectionString)
        {
            services.AddSingleton<IRedisCache>(x => new RedisCache(cacheDirectory, redisConnectionString));
        }
    }
}
