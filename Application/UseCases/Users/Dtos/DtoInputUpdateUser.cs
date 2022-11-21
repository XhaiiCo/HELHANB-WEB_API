using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Users.Dtos;

public class DtoInputUpdateUser
{
    [Required] public int Id { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    
    [EmailAddress]
    [Required] public string Email { get; set; }
}