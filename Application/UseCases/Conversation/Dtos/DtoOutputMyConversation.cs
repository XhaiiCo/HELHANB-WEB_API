using Application.UseCases.Ads.Dtos;

namespace Application.UseCases.Conversation.Dtos;

public class DtoOutputMyConversation
{
    public int Id { get; set; }
    public bool MessageNotView { get; set; }
    public DtoOutputUserInMyConversation Recipient { get; set; }

    public class DtoOutputUserInMyConversation
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePicturePath { get; set; }
    }
}