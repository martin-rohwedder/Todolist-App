using Application.Shared.Interfaces.Authentication;
using Application.Shared.Interfaces.Persistance;
using Infrastructure.Authentication;
using Infrastructure.DataAccess;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAuthServices(configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<TodoListDbContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString("TodoListDbConnectionString"),
                    x => x.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();

            return services;
        }

        private static IServiceCollection AddAuthServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            // Read and bind the JWT Settings from user secrets
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);
            services.AddSingleton(Options.Create(jwtSettings));

            // Add token service and set Bearer Token validation parameters
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecretKey!))
                });

            return services;
        }
    }
}
