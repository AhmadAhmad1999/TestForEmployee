using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Persistence.Repositories;

namespace SyriaTrustPlanning.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SyriaTrustPlanningDbContext>(options =>
                options.UseSqlServer(connectionString: configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            return services;
        }
    }
}
