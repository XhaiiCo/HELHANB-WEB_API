using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Domain;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;

namespace Application.UseCases.Users;

public class UseCaseUpdateUser : IUseCaseWriter<DtoOutputUser, DtoInputUpdateUser>
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;

    public UseCaseUpdateUser(IUserService userService,
        IUserRepository userRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
    }

    /// <summary>
    /// If the email isn't used, get the user, set the new data, update the user and return the user
    /// </summary>
    /// <param name="input">The input parameters of the command</param>
    /// <returns>
    /// A DtoOutputUser
    /// </returns>
    public DtoOutputUser Execute(DtoInputUpdateUser input)
    {
        User user;

        //Check if the email isn't already used
        try
        {
            user = _userService.FetchByEmail(input.Email);
            if (user != null && user.Id != input.Id)
                throw new Exception("Cette adresse email est déjà utilisée");
        }
        catch (KeyNotFoundException e)
        {
            user = _userService.FetchById(input.Id);
        }

        //Set the new data
        user.Email = input.Email;
        user.FirstName = input.FirstName;
        user.LastName = input.LastName;

        //Update the user
        _userRepository.Update(Mapper.GetInstance().Map<DbUser>(user));

        return Mapper.GetInstance().Map<DtoOutputUser>(user);
    }
}