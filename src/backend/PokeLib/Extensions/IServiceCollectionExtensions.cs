using Microsoft.Extensions.DependencyInjection;
using PokeLib.Services;

namespace PokeLib.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddPokeCache(this IServiceCollection services, string cacheDirectory)
        {
            services.AddSingleton<IPokeCache>(x => new PokeCache(cacheDirectory));
        }
    }
}
