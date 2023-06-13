using Application.Shared.Interfaces.Utilities;
using Application.Shared.Services.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Application
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add MediatR Service
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            // Add Password Hash Service
            services.AddSingleton<IPasswordHashService, PasswordHashService>();

            return services;
        }
    }
}
