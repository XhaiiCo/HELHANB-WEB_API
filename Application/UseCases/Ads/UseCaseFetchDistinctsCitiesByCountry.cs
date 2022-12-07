using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseFetchDistinctsCitiesByCountry: IUseCaseParameterizedQuery<IEnumerable<string>, string>
{
    private readonly IAdRepository _adRepository;

    public UseCaseFetchDistinctsCitiesByCountry(IAdRepository adRepository)
    {
        _adRepository = adRepository;
    }

    public IEnumerable<string> Execute(string country)
    {
        return _adRepository.FetchDistinctsCitiesByCountry(country);
    }
}