using Application.UserDetail.Commands.ChangePassword;
using Application.UserDetail.Commands.UpdateUser;
using Application.UserDetail.Shared;
using Mapster;
using WebApi.Contracts.UserDetail;

namespace WebApi.Shared.Mapping
{
    public class UserMappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserResult, UserDetailResponse>()
                .Map(dest => dest, src => src.User);

            config.NewConfig<UpdateUserRequest, UpdateUserCommand>()
                .Map(dest => dest.Username, src => MapContext.Current!.Parameters["username"]);

            config.NewConfig<ChangePasswordRequest, ChangePasswordCommand>()
                .Map(dest => dest.Username, src => MapContext.Current!.Parameters["username"]);
        }
    }
}
