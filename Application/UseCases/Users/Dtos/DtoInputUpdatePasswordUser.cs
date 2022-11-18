using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Users.Dtos;

public class DtoInputUpdatePasswordUser
{
    [Required] public int Id { get; set; }
    
    [MinLength(6)]
    [Required] public string Password { get; set; }
}