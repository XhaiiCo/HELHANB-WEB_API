namespace Domain;

public class Picture
{
    public int Id { get; set; }

    public string Path { get; set; }

    /// <summary>
    /// If the path of the picture is the same as the path of the picture that is being compared to, then return true
    /// </summary>
    /// <param name="picture">The picture to compare to.</param>
    /// <returns>
    /// Boolean value
    /// </returns>
    public bool Equals(Picture picture)
    {
        return picture.Path == this.Path;
    }
}