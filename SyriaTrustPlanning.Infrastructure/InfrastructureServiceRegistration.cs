using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SyriaTrustPlanning.Application.Contract.Infrastructure;
using SyriaTrustPlanning.Infrastructure.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyriaTrustPlanning.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJwtProvider, JwtProvider>();

            return services;
        }
    }
}
