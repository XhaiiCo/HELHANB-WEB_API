
namespace Application.UseCases.Ads.Dtos;

public class DtoOutputFilterData
{
    public IEnumerable<DtoOutputCountry> countries { get; set; }

    public class DtoOutputCountry
    {
        public string country { get; set; }
        public IEnumerable<string> city { get; set; }
    }
}