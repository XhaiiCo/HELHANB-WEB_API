using Infrastructure.Utils;

namespace API;

public class ConnectionStringProvider : IConnectionStringProvider
{
    private readonly IConfiguration _configuration;

    public ConnectionStringProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Get(string key)
    {
        return _configuration.GetConnectionString(key);
    }
}