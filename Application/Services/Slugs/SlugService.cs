using System.Text.RegularExpressions;

namespace API.Services;

public class SlugService : ISlugService
{
    public string RemoveAccent(string txt)
    {
        var bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
        return System.Text.Encoding.ASCII.GetString(bytes);
    }

    /// <summary>
    /// It generates a new GUID
    /// </summary>
    /// <returns>
    /// A string representation of a new GUID.
    /// </returns>
    public string GenerateUUID()
    {
        var uuid = Guid.NewGuid();
        return uuid.ToString();
    }

    /// <summary>
    /// 1. Remove accents from the string
    /// 2. Remove invalid characters
    /// 3. Convert multiple spaces into one space
    /// 4. Cut and trim the string to 50 characters
    /// 5. Replace spaces with hyphens
    /// 6. Append a UUID to the end of the string
    /// </summary>
    /// <param name="txt">The text to be converted to a slug.</param>
    /// <returns>
    /// A string that is the slug of the title.
    /// </returns>
    public string GenerateSlug(string txt)
    {
        var str = RemoveAccent(txt).ToLower();
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