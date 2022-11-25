using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Users.Dtos;

public class DtoInputFilteringUsers
{
    public string? Search { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed ")]
    public string? Role { get; set; }
}