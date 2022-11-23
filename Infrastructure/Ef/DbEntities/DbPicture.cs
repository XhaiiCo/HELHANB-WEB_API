namespace Infrastructure.Ef.DbEntities;

public class DbAdPicture
{
    public int Id { get; set; }
    
    public string Path { get; set; }
    
    public int AdId { get; set; }
}