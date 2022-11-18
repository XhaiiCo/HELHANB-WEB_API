namespace Application.Services.Ad;

public interface IAdService
{
    Domain.Ad FetchById(int id);
}