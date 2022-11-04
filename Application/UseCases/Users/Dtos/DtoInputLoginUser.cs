using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Users.Dtos;

public class DtoInputLoginUser
{
    [EmailAddress]
    [Required] public string Email { get; set; }
    
    [MinLength(6)]
    [Required] public string password { get; set; }
}