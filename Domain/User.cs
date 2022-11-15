namespace Domain;

public class User
{

    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime AccountCreation { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? ProfilePicturePath { get; set; }
    
    public Role Role { get; set; }

    public List<Ad> _Ads;
    public List<Ad> Ads
    {
        get => _Ads;
        set
        {
            value.ForEach(ad => AddAd(ad));
        }
    }

    public bool AddAd(Ad ad)
    {
        if (this.Role?.Id != 2) return false;
        
        this.Ads.Add(ad);

        return true;
    }
}