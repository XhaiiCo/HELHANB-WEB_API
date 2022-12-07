using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases;

public class UseCaseFetchDistinctsCountries:IUseCaseQuery<IEnumerable<string>>
{
    private readonly IAdRepository _adRepository;

    public UseCaseFetchDistinctsCountries(IAdRepository adRepository)
    {
        _adRepository = adRepository;
    }

    public IEnumerable<string> Execute()
    {
        return _adRepository.FetchDistinctsCountries();
    }
}