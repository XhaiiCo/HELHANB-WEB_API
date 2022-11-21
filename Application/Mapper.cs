using Application.UseCases.Ads.Dtos;
using Application.UseCases.Reservations.Dtos;
using Application.UseCases.Roles.Dtos;
using Application.UseCases.Users.Dtos;
using AutoMapper;
using Domain;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.User;

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

            //Ad
            cfg.CreateMap<DtoInputCreateAd, Ad>();
            cfg.CreateMap<DtoInputCreateAd, DbAd>();
            cfg.CreateMap<DbAd, DtoOutputAd>();
            cfg.CreateMap<Ad, DbAd>();
            cfg.CreateMap<DbAd, Ad>();

            //Time
            cfg.CreateMap<DtoInputTime, TimeSpan>();

            //Date
            cfg.CreateMap<DtoInputDateOnly, DateTime>();

            //Reservations
            cfg.CreateMap<DtoInputCreateReservation, DbReservation>();
            cfg.CreateMap<DbReservation, DtoOutputReservation>();
            cfg.CreateMap<DbUser, DtoOutputReservation.DtoRenter>();
            cfg.CreateMap<DbReservationStatus, DtoOutputReservation.DtoReservationStatus>();
            cfg.CreateMap<DbAd, DtoOutputReservation.DtoAd>();
            cfg.CreateMap<DbReservation, Reservation>();
        });
        return new AutoMapper.Mapper(config);
    }
}