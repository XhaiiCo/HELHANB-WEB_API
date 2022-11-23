using Infrastructure.Ef.DbEntities;
using Infrastructure.Utils;

namespace Infrastructure.Ef.Repository.HouseFeature;

public class HouseFeatureRepository: IHouseFeatureRepository
{
    private readonly HelhanbContextProvider _contextProvider;

    public HouseFeatureRepository(HelhanbContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public DbHouseFeature Create(DbHouseFeature dbHouseFeature)
    {
        using var context = _contextProvider.NewContext();
        context.HouseFeatures.Add(dbHouseFeature);
        context.SaveChanges();
        return dbHouseFeature;
    }
}