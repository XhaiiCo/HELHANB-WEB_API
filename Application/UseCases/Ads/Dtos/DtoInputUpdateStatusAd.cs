using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.Ads.Dtos;

public class DtoInputUpdateStatusAd
{
    [JsonIgnore] public int UserId { get; set; }
    [Required] public string AdSlug { get; set; }
    [Required] public int StatusId { get; set; }
}