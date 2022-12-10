using Application.UseCases.Conversation.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.conversation;
using Infrastructure.Ef.Repository.conversation.Message;

namespace Application.UseCases.Conversation;

public class UseCasePutMessageViewToTrue : IUseCaseWriter<bool, DtoInputPutMessageViewToTrue>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IMessageRepository _messageRepository;

    public UseCasePutMessageViewToTrue(
        IConversationRepository conversationRepository,
        IMessageRepository messageRepository
    )
    {
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
    }

    public bool Execute(DtoInputPutMessageViewToTrue dto)
    {
        var dbConversation = _conversationRepository.FetchById(dto.conversationId);

        //Check if the user belong to the conversation
        if (dto.UserId != dbConversation.IdUser1 && dto.UserId != dbConversation.IdUser2)
            throw new Exception("Vous ne faites pas partie de la conversation");

        var messages = _messageRepository.FetchByConversationidNotView(dto.conversationId);

        foreach (var message in messages)
        {
            if (message.SenderId != dto.UserId)
                _messageRepository.UpdateMessageViewToTrue(message.Id);
        }

        return true;
    }
}