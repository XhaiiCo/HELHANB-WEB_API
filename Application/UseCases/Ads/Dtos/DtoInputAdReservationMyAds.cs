namespace Application.UseCases.Ads.Dtos;

public class DtoInputAdReservationMyAds {
    public int Id { get; set; }
    public DateTime Creation { get; set; }
    public string adSlug { get; set; }
    public DateTime ArrivalDate { get; set; }
    public DateTime LeaveDate { get; set; }

    public DtoOutputMyAdsAd.DtoOutputAdReservationMyAds.DtoReservationStatusMyAds StatusMyAds { get; set; }
    public DtoOutputMyAdsAd.DtoOutputAdReservationMyAds.DtoRenterMyAds RenterMyAds { get; set; }

}