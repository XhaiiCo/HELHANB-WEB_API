using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef.Repository.HouseFeature;

public interface IHouseFeatureRepository
{
    public DbHouseFeature Create(DbHouseFeature dbHouseFeature) ;
    public IEnumerable<DbHouseFeature> FetchByAdId(int adId);
}