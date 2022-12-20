using Application.UseCases.Ads.Dtos;
using Application.UseCases.Conversation.Dtos;
using Application.UseCases.Reservations.Dtos;
using Application.UseCases.Roles.Dtos;
using Application.UseCases.Users.Dtos;
using AutoMapper;
using Domain;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository.Ad;
using Infrastructure.Ef.Repository.Reservation;
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
            cfg.CreateMap<User, DtoOutputUserLogin>();
            cfg.CreateMap<DbUser, User>();
            cfg.CreateMap<DtoOutputUser, DtoTokenUser>();
            cfg.CreateMap<User, DbUser>();
            cfg.CreateMap<DbUser, DtoOutputMyAdsAd.DtoOutputAdReservationMyAds.DtoRenterMyAds>();

            //Role
            cfg.CreateMap<DbRole, DtoOutputRole>();
            cfg.CreateMap<Role, DtoOutputRole>();

            //Ad
            cfg.CreateMap<DtoInputCreateAd, Ad>();
            cfg.CreateMap<DtoInputCreateAd, DbAd>();
            cfg.CreateMap<DbAd, DtoOutputAd>();
            cfg.CreateMap<Ad, DtoOutputAd>();
            cfg.CreateMap<Ad, DbAd>();
            cfg.CreateMap<DbAd, Ad>();
            cfg.CreateMap<User, DtoOutputUserInAd>();
            cfg.CreateMap<Ad, DtoOutputAdWithReservations>();
            cfg.CreateMap<Ad, DtoOutputAdsSummary>();
            cfg.CreateMap<Ad, DtoOutputMyAdsAd>();
            cfg.CreateMap<DtoInputFilteringAds, FilteringAd>();
            cfg.CreateMap<Reservation, DtoOutputUserReservation>();
            cfg.CreateMap<DbReservation, DtoOutputMyAdsAd.DtoOutputAdReservationMyAds>();

            //AdStatus
            cfg.CreateMap<DbAdStatus, AdStatus>();
            cfg.CreateMap<AdStatus, DtoOutputAdStatus>();

            //Time
            cfg.CreateMap<DtoInputTime, TimeSpan>();
            cfg.CreateMap<TimeSpan, TimeSpan>();

            //Date
            cfg.CreateMap<DtoInputDateOnly, DateTime>();

            //Reservations
            cfg.CreateMap<DtoInputCreateReservation, DbReservation>();
            cfg.CreateMap<DbReservation, DtoOutputReservation>();
            cfg.CreateMap<DbUser, DtoOutputReservation.DtoRenter>();
            cfg.CreateMap<DbReservationStatus, DtoOutputReservation.DtoReservationStatus>();
            cfg.CreateMap<DbAd, DtoOutputReservation.DtoAd>();
            cfg.CreateMap<DbReservation, Reservation>();
            cfg.CreateMap<DbReservationStatus,
                DtoOutputMyAdsAd.DtoOutputAdReservationMyAds.DtoReservationStatusMyAds>();
            cfg.CreateMap<Reservation, DtoOutputReservation>();
            cfg.CreateMap<User, DtoOutputReservation.DtoRenter>();
            cfg.CreateMap<Ad, DtoOutputReservation.DtoAd>();
            cfg.CreateMap<Reservation, DtoOutputMyAdReservation>();
            cfg.CreateMap<ReservationStatus, DtoOutputReservation.DtoReservationStatus>();
            cfg.CreateMap<DbReservationStatus, ReservationStatus>();

            //AdPicture
            cfg.CreateMap<DtoInputAddPictureAd, DbAdPicture>();
            cfg.CreateMap<DbAdPicture, DtoOutputAdPicture>();
            cfg.CreateMap<DbAdPicture, Picture>();
            cfg.CreateMap<Picture, DtoOutputAdPicture>();

            //Conversation
            cfg.CreateMap<DtoInputCreateConversation, DbConversation>();
            cfg.CreateMap<DbConversation, DtoOutputCreatedConversation>();
            cfg.CreateMap<User, DtoOutputMyConversation.DtoOutputUserInMyConversation>();
            cfg.CreateMap<DbMessage, DtoOutputMessage>();

            //Message
            cfg.CreateMap<DtoInputCreateMessage, DbMessage>();
            cfg.CreateMap<DbMessage, DtoOutputCreatedMessage>();
        });
        return new AutoMapper.Mapper(config);
    }
}