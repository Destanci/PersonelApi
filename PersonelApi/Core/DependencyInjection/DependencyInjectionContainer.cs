using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonelApi.DataAccess;
using PersonelApi.Model;

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
            services.AddDbContext<DbContext, PersonelContext>();

            services.AddScoped<EfEmployeeDal>();

            return services;
        }
    }
}
