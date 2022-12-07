namespace API.Services;

public interface ISlugService
{
    string RemoveAccent(string txt);

    string GenerateUUID();

    string GenerateSlug(string txt);
}