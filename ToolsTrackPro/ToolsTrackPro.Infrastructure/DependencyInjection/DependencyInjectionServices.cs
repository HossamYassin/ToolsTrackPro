using Microsoft.Extensions.DependencyInjection;
using ToolsTrackPro.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using ToolsTrackPro.Infrastructure.Interfaces;

namespace ToolsTrackPro.Infrastructure.DependencyInjection
{
    public static class DependencyInjectionServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Register repositories with connection string
            services.AddScoped<INotificationRepository>(sp => new NotificationRepository(connectionString));
            services.AddScoped<IToolRepository>(sp => new ToolRepository(connectionString));
            services.AddScoped<ITransactionRepository>(sp => new TransactionRepository(connectionString));
            services.AddScoped<IUserRepository>(sp => new UserRepository(connectionString));

            return services;
        }
    }
}
