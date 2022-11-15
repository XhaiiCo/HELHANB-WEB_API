using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Ef.DbEntities;

public class DbMessage
{
    public int Id { get; set; }
    
    public int SenderId { get; set; }
    
    public string Content { get; set; }
   
    public bool View { get; set; }
    public DateTime SendTime { get; set; }
    
}