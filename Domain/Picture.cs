namespace Domain;

public class Picture
{
    public int Id { get; set; }
    
    public string Path { get; set; }

    public bool Equals(Picture picture)
    {
        if (picture.Path == this.Path) return true;

        return false;
    }
    
}