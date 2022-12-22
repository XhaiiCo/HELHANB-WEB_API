using API.Utils.Picture;
using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;

namespace Application.UseCases.Users;

public class UseCaseUpdateProfilePictureBase64 : IUseCaseWriter<DtoOutputUser, DtoInputProfilePictureBase64>
{
    private readonly IUserService _userService;
    private readonly IPictureService _pictureService;
    private readonly UseCaseUpdateUserProfilePicture _useCaseUpdateUserProfilePicture;

    public UseCaseUpdateProfilePictureBase64(IUserService userService, IPictureService pictureService,
        UseCaseUpdateUserProfilePicture useCaseUpdateUserProfilePicture)
    {
        _userService = userService;
        _pictureService = pictureService;
        _useCaseUpdateUserProfilePicture = useCaseUpdateUserProfilePicture;
    }

    public DtoOutputUser Execute(DtoInputProfilePictureBase64 dto)
    {
        var currentUser = _userService.FetchById(dto.userId);
        var filePath = "\\Upload\\ProfilePicture\\default_user_pic.png";


        if (dto.ProfilePicture != null)
        {
            const string basePath = "\\Upload\\ProfilePicture\\";

            filePath = basePath + _pictureService.GenerateUniqueFileName(dto.userId) +
                       _pictureService.GetExtensionOfBase64(dto.ProfilePicture);

            _pictureService.UploadBase64Picture(basePath, filePath, dto.ProfilePicture);
        }

        var dtoInputUpdateProfilePictureUser = new DtoInputUpdateProfilePictureUser
        {
            Id = dto.userId,
            ProfilePicturePath = filePath
        };
        var oldProfilePicturePath = currentUser.ProfilePicturePath;

        var user = _useCaseUpdateUserProfilePicture.Execute(dtoInputUpdateProfilePictureUser);

        //Remove the current profile picture if exist
        if (oldProfilePicturePath != null && oldProfilePicturePath !=
            "\\Upload\\ProfilePicture\\default_user_pic.png")
        {
            _pictureService.RemoveFile(oldProfilePicturePath);
        }

        return user;
    }
}