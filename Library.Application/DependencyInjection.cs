using Library.Application.Common.Behaviors;

namespace Library.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(assembly);
                configuration.Lifetime = ServiceLifetime.Scoped;
            });

            services.AddValidatorsFromAssembly(assembly);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
