﻿using Infrastructure.Ef.DbEntities;
using Infrastructure.Utils;

namespace Infrastructure.Ef.Repository;

public class RoleRepository: IRoleRepository
{
    private readonly HelhanbContextProvider _contextProvider;

    public RoleRepository(HelhanbContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public IEnumerable<DbRole> FetchAll()
    {
        using var context = _contextProvider.NewContext();

        return context.Roles.ToList();
    }
}