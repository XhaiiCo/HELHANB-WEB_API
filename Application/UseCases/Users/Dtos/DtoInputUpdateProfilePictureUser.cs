using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Users.Dtos;

public class DtoInputUpdateProfilePictureUser
{
    public int Id { get; set; }
    public string? ProfilePicturePath { get; set; }
}