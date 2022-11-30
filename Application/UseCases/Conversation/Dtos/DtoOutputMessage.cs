namespace Application.UseCases.Conversation.Dtos;

public class DtoOutputMessage
{
    
    public int SenderId { get; set; }
    
    public string Content { get; set; }
    public DateTime SendTime { get; set; }
}