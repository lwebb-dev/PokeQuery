using Microsoft.Extensions.DependencyInjection;
using PokeLib.Services;

namespace PokeLib.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddPokeCache(this IServiceCollection services)
        {
            services.AddSingleton<IPokeCache, PokeCache>();
        }
    }
}
