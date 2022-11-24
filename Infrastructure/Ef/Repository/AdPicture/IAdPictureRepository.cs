using System.Data.Common;
using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef.Repository.AdPicture;

public interface IAdPictureRepository
{
    DbAdPicture Create(DbAdPicture dbAdPicture);
    IEnumerable<DbAdPicture> FetchByAdId(int adId); 
}