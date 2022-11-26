using Infrastructure.Ef.DbEntities;

namespace Infrastructure.Ef.Repository.Ad.AdStatus;

public interface IAdStatusRepository
{
    DbAdStatus FetchById(int id);
}