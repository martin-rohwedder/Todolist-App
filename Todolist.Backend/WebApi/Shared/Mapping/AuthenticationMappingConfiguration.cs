using Application.Authentication.Commands.Register;
using Application.Authentication.Queries.Login;
using Application.Authentication.Shared;
using Mapster;
using WebApi.Contracts.Authentication;

namespace WebApi.Shared.Mapping
{
    public class AuthenticationMappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequest, RegisterCommand>();
            config.NewConfig<LoginRequest, LoginQuery>();
            config.NewConfig<AuthenticationResult, AuthenticationResponse>()
                .Map(dest => dest, src => src.User);
        }
    }
}