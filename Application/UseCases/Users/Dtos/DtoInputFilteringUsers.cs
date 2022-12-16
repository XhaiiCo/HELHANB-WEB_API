using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Users.Dtos;

public class DtoInputFilteringUsers
{
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public string? Search { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed ")]
    public string? Role { get; set; }
}