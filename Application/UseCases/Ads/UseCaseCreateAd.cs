using Application.UseCases.Ads.Dtos;
using Infrastructure.Ef.Repository.Ad;

namespace Application.UseCases.Ads;

public class UseCaseCreateAd
{
    private readonly IAdRepository _adRepository;
    
    public UseCaseCreateAd(IAdRepository adRepository)
    {
        _adRepository = adRepository;
    }
    
    public DtoOutputAd Execute(DtoInputCreateAd input)
    {
        var mapper = Mapper.GetInstance();

        var ad = mapper.Map<Domain.Ad>(input);
        var dbAd = _adRepository.Create
        (ad.Id,ad.Name, ad.PricePerNight, ad.Description, ad.NumberOfPersons, ad.NumberOfBedrooms, ad.Street, ad.PostalCode, ad.Country,
            ad.City);
        return Mapper.GetInstance().Map<DtoOutputAd>(dbAd);
    }
}