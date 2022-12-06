using Application.UseCases.Ads.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseFetchFilterData : IUseCaseQuery<DtoOutputFilterData>
{
    private readonly IAdRepository _adRepository;

    public UseCaseFetchFilterData(IAdRepository adRepository)
    {
        _adRepository = adRepository;
    }

    public DtoOutputFilterData Execute()
    {
        var countries = _adRepository.FetchDistinctByCountry();

        DtoOutputFilterData result = new DtoOutputFilterData
        {
            countries = new List<DtoOutputFilterData.DtoOutputCountry>()
        };

        foreach (var country in countries)
        {
            var newCountry = new DtoOutputFilterData.DtoOutputCountry
            {
                country = country,
                city = new List<string>()
            };

            var cities = _adRepository.FetchByCountryDistinctCity(country);
            foreach (var city in cities)
            {
                newCountry.city.Append(city);
            }

            result.countries.Append(newCountry);
        }

        return result;
    }
}