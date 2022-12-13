using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.Users.Dtos;

public class DtoInputProfilePictureBase64
{
    [JsonIgnore] public int userId { get; set; }
    public string? ProfilePicture { get; set; }
}