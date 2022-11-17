using Application.Services.User;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef;

namespace Application.UseCases.Users;

public class UseCaseFetchAllUsers: IUseCaseParameterizedQuery<IEnumerable<DtoOutputUser>, DtoInputFilteringUsers>
{

    private readonly IUserService _userService;

    public UseCaseFetchAllUsers(IUserService userService)
    {
        _userService = userService;
    }

    public IEnumerable<DtoOutputUser> Execute(DtoInputFilteringUsers param)
    {
        var users = _userService.FetchAll();
        
        if(param.Role != null)
            users = users.Where(user => user.Role.Name == param.Role);

        if (param.Search != null)
            users = users.Where(user => user.FirstName.ToLower().Contains(param.Search.ToLower()) ||
                                        user.LastName.ToLower().Contains(param.Search.ToLower()) ||
                                        user.Email.ToLower().Contains(param.Search.ToLower()));
        
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputUser>>(users);
    }
}