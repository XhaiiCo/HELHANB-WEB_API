using System.Text;
using API;
using API.ChatController;
using API.Services;
using API.Utils.Picture;
using Application.Services;
using Application.Services.Ad;
using Application.Services.Auth;
using Application.Services.Date;
using Application.Services.Reservation;
using Application.Services.ReservationBook;
using Application.Services.Role;
using Application.Services.Time;
using Application.Services.Token;
using Application.Services.User;
using Application.UseCases;
using Application.UseCases.Ads;
using Application.UseCases.Conversation;
using Application.UseCases.Reservations;
using Application.UseCases.Roles;
using Application.UseCases.Users;
using Application.UseCases.Users.Dtos;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository;
using Infrastructure.Ef.Repository.Ad;
using Infrastructure.Ef.Repository.Ad.AdStatus;
using Infrastructure.Ef.Repository.AdPicture;
using Infrastructure.Ef.Repository.AdStatus;
using Infrastructure.Ef.Repository.conversation;
using Infrastructure.Ef.Repository.conversation.Message;
using Infrastructure.Ef.Repository.HouseFeature;
using Infrastructure.Ef.Repository.Reservation;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new
            SymmetricSecurityKey
            (Encoding.UTF8.GetBytes
                (builder.Configuration["Jwt:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["jwt"];
            return Task.CompletedTask;
        },
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("https://localhost:4200").AllowAnyHeader().AllowAnyMethod()
                .AllowCredentials();
        });
});
builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();
builder.Services.AddScoped<HelhanbContextProvider>();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IAdRepository, AdRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationStatusRepository, ReservationStatusRepository>();
builder.Services.AddScoped<IAdPictureRepository, AdPictureRepository>();
builder.Services.AddScoped<IHouseFeatureRepository, HouseFeatureRepository>();
builder.Services.AddScoped<IAdStatusRepository, AdStatusRepository>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

// Users
builder.Services.AddScoped<UseCaseFetchAllUsers>();
builder.Services.AddScoped<UseCaseCreateUser>();
builder.Services.AddScoped<UseCaseLoginUser>();
builder.Services.AddScoped<UseCaseUpdateUserProfilePicture>();
builder.Services.AddScoped<UseCaseFetchUserById>();
builder.Services.AddScoped<UseCaseDeleteUserById>();
builder.Services.AddScoped<UseCaseUpdatePasswordUser>();
builder.Services.AddScoped<UseCaseUpdateUser>();
builder.Services.AddScoped<UseCaseChangeRoleToHostUser>();
builder.Services.AddScoped<UseCaseChangeRole>();
builder.Services.AddScoped<UseCaseUpdateProfilePictureBase64>();
builder.Services.AddScoped<UseCaseCountUsers>();

//Reservation
builder.Services.AddScoped<UseCaseCreateReservation>();

// Roles
builder.Services.AddScoped<UseCaseFetchAllRoles>();

//Ads
builder.Services.AddScoped<UseCaseCreateAd>();
builder.Services.AddScoped<UseCaseDeleteAd>();
builder.Services.AddScoped<UseCaseFetchAllAds>();
builder.Services.AddScoped<UseCaseAddPictureAd>();
builder.Services.AddScoped<UseCaseFetchAdBySlug>();
builder.Services.AddScoped<UseCaseCountValidatedAds>();
builder.Services.AddScoped<UseCaseFetchAdsForPagination>();
builder.Services.AddScoped<UseCaseUpdateStatusAd>();
builder.Services.AddScoped<UseCaseFetchByUserIdAd>();
builder.Services.AddScoped<UseCaseUpdateAd>();
builder.Services.AddScoped<UseCaseFetchMyReservations>();
builder.Services.AddScoped<UseCaseRemoveReservation>();
builder.Services.AddScoped<UseCaseFetchDistinctsCountries>();
builder.Services.AddScoped<UseCaseFetchDistinctsCitiesByCountry>();
builder.Services.AddScoped<UseCaseFetchAllReservationByAd>();
builder.Services.AddScoped<UseCaseConfirmReservation>();
builder.Services.AddScoped<UseCaseRefuseReservation>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPictureService, PictureService>();
builder.Services.AddScoped<ITimeService, TimeService>();
builder.Services.AddScoped<IAdService, AdService>();
builder.Services.AddScoped<IReservationBookService, ReservationBookService>();
builder.Services.AddScoped<IDateService, DateService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ISlugService, SlugService>();

//conversation
builder.Services.AddScoped<UseCaseCreateConversation>();
builder.Services.AddScoped<UseCaseCreateMessage>();
builder.Services.AddScoped<UseCaseFetchMyConversation>();
builder.Services.AddScoped<UseCaseFetchMessageForAConversation>();
builder.Services.AddScoped<UseCasePutMessageViewToTrue>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Pour android en local
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("Dev");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();
app.MapHub<ChatHub>("/chatsocket");

app.Run();