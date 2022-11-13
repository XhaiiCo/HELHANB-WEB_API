using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;

namespace Application.UseCases.Users;

public class UseCaseUpdateUserProfilePicture: IUseCaseWriter<DtoOutputUser, DtoInputUpdateProfilePictureUser>
{
    private IUserService _userService;
    private IUserRepository _userRepository;

    public UseCaseUpdateUserProfilePicture(IUserService userService, IUserRepository userRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
    }

    public DtoOutputUser Execute(DtoInputUpdateProfilePictureUser input)
    {
        var user = _userService.FetchById(input.Id);
        user.ProfilePicturePath = input.ProfilePicturePath;

        var dbUser = Mapper.GetInstance().Map<DbUser>(user);
        var updatedUser = _userService.MapToUser(_userRepository.Update(dbUser)) ;

        var dtoUser =  Mapper.GetInstance().Map<DtoOutputUser>(updatedUser);
        
        return dtoUser;
    }
}