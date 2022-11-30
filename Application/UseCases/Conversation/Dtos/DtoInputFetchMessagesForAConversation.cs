namespace Application.UseCases.Conversation.Dtos;

public class DtoInputFetchMessagesForAConversation
{
    public int UserId { get; set; }
    public int conversationId { get; set; }
}