using System.Text.RegularExpressions;

namespace API.Services;

public class SlugService : ISlugService
{
    public string RemoveAccent(string txt) 
    { 
        byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt); 
        return System.Text.Encoding.ASCII.GetString(bytes); 
    }

    //generate unique id
    public string GenerateUUID()
    {
        Guid uuid = Guid.NewGuid();
        return uuid.ToString();
    }
    
    public string GenerateSlug(string txt) 
    {
        string str = RemoveAccent(txt).ToLower(); 
        // invalid chars           
        str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); 
        // convert multiple spaces into one space   
        str = Regex.Replace(str, @"\s+", " ").Trim(); 
        // cut and trim 
        str = str.Substring(0, str.Length <= 50 ? str.Length : 50).Trim();   
        str = Regex.Replace(str, @"\s", "-"); // hyphens   
        return str + "-" + GenerateUUID(); 
    }
}