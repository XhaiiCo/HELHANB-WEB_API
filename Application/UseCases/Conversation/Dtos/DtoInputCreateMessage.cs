namespace Application.UseCases.Conversation.Dtos;

public class DtoInputCreateMessage
{

    public int conversationId { get; set; }
    
    public int SenderId { get; set; }
    
    public string Content { get; set; }
}