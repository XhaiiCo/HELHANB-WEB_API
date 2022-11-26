using Infrastructure.Ef.DbEntities;
using Infrastructure.Utils;

namespace Infrastructure.Ef.Repository.Ad.AdStatus;

public class AdStatusRepository: IAdStatusRepository
{
    private readonly HelhanbContextProvider _contextProvider;

    public AdStatusRepository(HelhanbContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public DbAdStatus FetchById(int id)
    {
        using var context = _contextProvider.NewContext();
        var dbAdStatus = context.AdStatus.FirstOrDefault(adStatus => adStatus.Id == id);
        
        if (dbAdStatus == null)
            throw new KeyNotFoundException($"Ad status with id {id} has not been found");

        return dbAdStatus;
    }
}