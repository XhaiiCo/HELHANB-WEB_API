using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Ads.Dtos;

public class DtoInputTime
{
    [Required] public int Hours { get; set; }
    [Required] public int Minutes { get; set; }
}