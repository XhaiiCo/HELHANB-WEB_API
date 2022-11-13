using System.Data.Common;
using Application.UseCases.Roles.Dtos;
using Application.UseCases.Users.Dtos;
using AutoMapper;
using Domain;
using Infrastructure.Ef.DbEntities;

namespace Application;

public class Mapper
{
    private static AutoMapper.Mapper _instance;

    public static AutoMapper.Mapper GetInstance()
    {
        return _instance ??= CreateMapper();
    }

    private static AutoMapper.Mapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {

            // User
            cfg.CreateMap<User, DtoOutputUser>();
            cfg.CreateMap<DtoInputCreateUser, DbUser>();
            cfg.CreateMap<DbUser, DtoOutputUser>();
            cfg.CreateMap<DbUser, User>();
            cfg.CreateMap<DtoOutputUser, DtoTokenUser>();
            cfg.CreateMap<User, DbUser>();
            
            //Role
            cfg.CreateMap<DbRole, DtoOutputRole>();
            cfg.CreateMap<Role, DtoOutputRole>();
        });
        return new AutoMapper.Mapper(config);
    }
}