using Infrastructure.Ef;

namespace Infrastructure.Utils;

public class HelhanbContextProvider
{
    private readonly IConnectionStringProvider _connectionStringProvider;

    public HelhanbContextProvider(IConnectionStringProvider connectionStringProvider)
    {
        _connectionStringProvider = connectionStringProvider;
    }

    public HelhanbContext NewContext()
    {
        return new HelhanbContext(_connectionStringProvider);
    }
}