using Application.Shared.Interfaces.Authentication;
using Application.Shared.Interfaces.Persistance;
using Infrastructure.Authentication;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            //TODO: Move all constants to a JWT Settings class, which reads from user secrets 
            // Add token service and set Bearer Token validation parameters
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Todolist API",
                    ValidAudience = "Todolist API",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("super-secret-keysuper-secret-key"))
                });

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
