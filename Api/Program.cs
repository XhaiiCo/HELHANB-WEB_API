using System.Text;
using API;
using API.Utils.Picture;
using Application.Services.Auth;
using Application.Services.Token;
using Application.Services.User;
using Application.UseCases.Roles;
using Application.UseCases.Users;
using Infrastructure.Ef;
using Infrastructure.Ef.DbEntities;
using Infrastructure.Ef.Repository;
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
    options.AddPolicy("Dev", policyBuilder =>
    {
        policyBuilder.WithOrigins("https://localhost:4200").
            AllowAnyHeader().
            AllowAnyMethod().
            AllowCredentials();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();
builder.Services.AddScoped<HelhanbContextProvider>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Users
builder.Services.AddScoped<UseCaseFetchAllUsers>();
builder.Services.AddScoped<UseCaseCreateUser>();
builder.Services.AddScoped<UseCaseLoginUser>();
builder.Services.AddScoped<UseCaseUpdateUserProfilePicture>();
builder.Services.AddScoped<UseCaseFetchUserById>();
builder.Services.AddScoped<UseCaseDeleteUserById>();

// Roles
builder.Services.AddScoped<UseCaseFetchAllRoles>() ;

//Services
builder.Services.AddScoped<IUserService, UserService>() ;
builder.Services.AddScoped<IAuthService, AuthService>() ;
builder.Services.AddScoped<ITokenService, TokenService>() ;
builder.Services.AddScoped<IPictureService, PictureService>() ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Dev");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();