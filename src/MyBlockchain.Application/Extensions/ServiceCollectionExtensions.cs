using Microsoft.Extensions.DependencyInjection;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Application.Services;
using MyBlockchain.Infrastructure.Interfaces;
using MyBlockchain.Infrastructure.Repositories;
using MyBlockchain.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEthBlockService, EthBlockService>();
            services.AddScoped<IDashBlockService, DashBlockService>();
            services.AddScoped<IBtcBlockService, BtcBlockService>();
            services.AddScoped<ILtcBlockService, LtcBlockService>();
            services.AddScoped<IBitCoinBlockService, BitCoinBlockService>();
            services.AddScoped<IApiAuditService, ApiAuditService>();

            return services;
        }

        public static IServiceCollection AddApplicationRepositories(this IServiceCollection services)
        {
            services.AddScoped<IDashBlockRepository, DashBlockRepository>();
            services.AddScoped<IBitCoinBlockRepository, BitCoinBlockRepository>();
            services.AddScoped<IBtcBlockRepository, BtcBlockRepository>();
            services.AddScoped<IEthBlockRepository, EthBlockRepository>();
            services.AddScoped<ILtcBlockRepository, LtcBlockRepository>();
            return services;
        }
    }
}
