using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Ads.Dtos;

public class DtoInputAddPictureAd
{
    
    [Required] public string Path { get; set; }
    
    [Required] public int AdId { get; set; }
}