namespace Infrastructure.Utils;

public interface IConnectionStringProvider
{
    string Get(string key);
}