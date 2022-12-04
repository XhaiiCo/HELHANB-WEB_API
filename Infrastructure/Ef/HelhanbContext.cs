using Infrastructure.Ef.DbEntities;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Ef;

public class HelhanbContext : DbContext
{
    private readonly IConnectionStringProvider _connectionStringProvider;

    public HelhanbContext(IConnectionStringProvider connectionStringProvider)
    {
        _connectionStringProvider = connectionStringProvider;
    }

    public DbSet<DbConversation> Conversations { get; set; }

    public DbSet<DbMessage> Messages { get; set; }

    public DbSet<DbHouseFeature> HouseFeatures { get; set; }
    public DbSet<DbAdPicture> AdPictures { get; set; }

    public DbSet<DbAd> Ads { get; set; }
    public DbSet<DbAdStatus> AdStatus { get; set; }
    public DbSet<DbRole> Roles { get; set; }
    public DbSet<DbUser> Users { get; set; }

    public DbSet<DbReservation> Reservations { get; set; }

    public DbSet<DbReservationStatus> ReservationStatus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(_connectionStringProvider.Get("db"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbConversation>(entity =>
        {
            entity.ToTable("conversations");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasColumnName("conversation_id");
            entity.Property(c => c.IdUser1).HasColumnName("id_user_1");
            entity.Property(c => c.IdUser2).HasColumnName("id_user_2");
        });

        modelBuilder.Entity<DbMessage>(entity =>
        {
            entity.ToTable("messages");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).HasColumnName("message_id");
            entity.Property(m => m.SenderId).HasColumnName("sender_id");
            entity.Property(m => m.Content).HasColumnName("content");
            entity.Property(m => m.View).HasColumnName("view_message");
            entity.Property(m => m.SendTime).HasColumnName("send_time");
            entity.Property(m => m.ConversationId).HasColumnName("conversation_id");
        });

        modelBuilder.Entity<DbHouseFeature>(entity =>
        {
            entity.ToTable("house_features");
            entity.HasKey(hF => hF.HouseFeatureId);
            entity.Property(hF => hF.HouseFeatureId).HasColumnName("house_feature_id");
            entity.Property(hF => hF.Feature).HasColumnName("feature");
            entity.Property(hF => hF.AdId).HasColumnName("ad_id");
        });

        modelBuilder.Entity<DbAdPicture>(entity =>
        {
            entity.ToTable("ad_pictures");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("picture_id");
            entity.Property(p => p.Path).HasColumnName("path");
            entity.Property(p => p.AdId).HasColumnName("ad_id");
        });

        modelBuilder.Entity<DbAdStatus>(entity =>
        {
            entity.ToTable("ad_status");
            entity.HasKey(adStatus => adStatus.Id);
            entity.Property(adStatus => adStatus.Id).HasColumnName("ad_status_id");
            entity.Property(adStatus => adStatus.StatusName).HasColumnName("status_name");
        });

        modelBuilder.Entity<DbAd>(entity =>
        {
            entity.ToTable("ads");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasColumnName("ad_id");
            entity.Property(a => a.Name).HasColumnName("ad_name");
            entity.Property(a => a.Created).HasColumnName("ad_created");
            entity.Property(a => a.PricePerNight).HasColumnName("price_per_night");
            entity.Property(a => a.Description).HasColumnName("description");
            entity.Property(a => a.NumberOfPersons).HasColumnName("number_of_persons");
            entity.Property(a => a.NumberOfBedrooms).HasColumnName("number_of_bedrooms");
            entity.Property(a => a.Street).HasColumnName("street");
            entity.Property(a => a.PostalCode).HasColumnName("postal_code");
            entity.Property(a => a.Country).HasColumnName("country");
            entity.Property(a => a.City).HasColumnName("city");
            entity.Property(a => a.UserId).HasColumnName("user_id");
            entity.Property(a => a.AdStatusId).HasColumnName("ad_status_id");
            entity.Property(a => a.ArrivalTimeRangeStart).HasColumnName("arrival_time_range_start");
            entity.Property(a => a.ArrivalTimeRangeEnd).HasColumnName("arrival_time_range_end");
            entity.Property(a => a.LeaveTime).HasColumnName("leave_time");
        });

        modelBuilder.Entity<DbRole>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).HasColumnName("role_id");
            entity.Property(r => r.Name).HasColumnName("name");
        });

        modelBuilder.Entity<DbUser>(entity =>
        {
            entity.ToTable("users");
            entity.ToTable(tb => tb.HasTrigger("DELETE-USER"));
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnName("user_id");
            entity.Property(u => u.FirstName).HasColumnName("first_name");
            entity.Property(u => u.LastName).HasColumnName("last_name");
            entity.Property(u => u.AccountCreation).HasColumnName("account_creation");
            entity.Property(u => u.Email).HasColumnName("email");
            entity.Property(u => u.Password).HasColumnName("password");
            entity.Property(u => u.RoleId).HasColumnName("role_id");
            entity.Property(u => u.ProfilePicturePath).HasColumnName("profile_picture_path");
        });

        modelBuilder.Entity<DbReservation>(entity =>
        {
            entity.ToTable("reservations");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).HasColumnName("reservation_id");
            entity.Property(r => r.Creation).HasColumnName("reservation_creation");
            entity.Property(r => r.ArrivalDate).HasColumnName("arrival_date");
            entity.Property(r => r.LeaveDate).HasColumnName("leave_date");
            entity.Property(r => r.ReservationStatusId).HasColumnName("reservation_status_id");
            entity.Property(r => r.AdId).HasColumnName("ad_id");
            entity.Property(r => r.RenterId).HasColumnName("renter");
        });

        modelBuilder.Entity<DbReservationStatus>(entity =>
        {
            entity.ToTable("reservation_status");
            entity.HasKey(rs => rs.Id);
            entity.Property(rs => rs.Id).HasColumnName("reservation_status_id");
            entity.Property(rs => rs.StatusName).HasColumnName("status_name");
        });
    }
}