using Microsoft.Extensions.DependencyInjection;
using PersonelApi.DataAccess;

namespace PersonelApi.Core.DependencyInjection
{
    public static class DependencyInjectionContainer
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddScoped<EfEmployeeDal>();

            return services;
        }
    }
}
