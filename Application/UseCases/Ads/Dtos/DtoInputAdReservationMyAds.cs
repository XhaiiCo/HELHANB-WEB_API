using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.Ads.Dtos;

public class DtoInputAdReservationMyAds
{
    [JsonIgnore] public int userId { get; set; }
    [Required] public int Id { get; set; }
    [Required] public string adSlug { get; set; }
}