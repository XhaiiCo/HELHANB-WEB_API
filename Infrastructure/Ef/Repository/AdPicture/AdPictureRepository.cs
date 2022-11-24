using Infrastructure.Ef.DbEntities;
using Infrastructure.Utils;

namespace Infrastructure.Ef.Repository.AdPicture;

public class AdPictureRepository: IAdPictureRepository
{

    private readonly HelhanbContextProvider _contextProvider;

    public AdPictureRepository(HelhanbContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public DbAdPicture Create(DbAdPicture dbAdPicture)
    {
        using var context = _contextProvider.NewContext();
        
        context.AdPictures.Add(dbAdPicture);
        context.SaveChanges();
        return dbAdPicture;
    }

    public IEnumerable<DbAdPicture> FetchByAdId(int adId)
    {
        using var context = _contextProvider.NewContext();

        return context.AdPictures.Where(picture => picture.AdId == adId).ToList();
    }
}