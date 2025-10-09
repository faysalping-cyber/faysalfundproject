using FaysalFundsInternal.Application.Services;
using FaysalFundsInternal.Domain.Interfaces;
using FaysalFundsInternal.Persistence.Context;
using FaysalFundsInternal.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FaysalFundsInternal.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Register EF Core DbContext, Repositories, etc.
            services.AddDbContext<ApplicationDbContext>(options => options.UseOracle(configuration.GetConnectionString("MW")));

            //Repositories
            services.AddScoped<IAccountStatementRepository, AccountStatementRepository>();
            services.AddScoped<IFamlFundDetailRepository, FamlFundDetailRepository>();
            services.AddScoped<IUHRRepository, UHRRepository>();
            services.AddScoped<IUHSRepository, UHSRepository>();
            services.AddScoped<IUnitHolderRepository, UnitHolderRepository>();

            //Services
            services.AddScoped<AccountStatementService>();
            services.AddScoped<UhsService>();
            services.AddScoped<UhrService>();
            ;            return services;
        }
    }
}
