using Library.Domain.Interfaces;
using Library.Infrastructure.Data;
using Library.Infrastructure.Data.Seeders;
using Library.Infrastructure.Repositories;

namespace Library.Infrastructure
{
    public static class DependencyInjection
    {
        /*
            # Crear primera migración
            dotnet ef migrations add InitialCreate --project Library.Infrastructure.Data --startup-project Library.Api

            # Actualizar base de datos
            dotnet ef database update --project Library.Infrastructure.Data --startup-project Library.Api

            # Ver migraciones pendientes
            dotnet ef migrations list --project Library.Infrastructure.Data --startup-project Library.Api

            # Generar script SQL
            dotnet ef migrations script --project Library.Infrastructure.Data --startup-project Library.Api
        */
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer
                (
                    configuration.GetConnectionString("Default"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                );

                options.EnableSensitiveDataLogging(false);
                options.EnableDetailedErrors(false);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }

        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();
            await DatabaseSeeder.SeedAsync(context);
        }
    }
}
