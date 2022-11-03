using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Users.Dtos;

public class DtoInputCreateUser
{
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public string password { get; set; }
    [Required] public string Email { get; set; }
    //[Required] public DateOnly BirthDate { get; set; }
}