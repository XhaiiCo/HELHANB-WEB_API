using Application.UseCases.Conversation.Dtos;
using Application.UseCases.Utils;
using Infrastructure.Ef.Repository.conversation;
using Infrastructure.Ef.Repository.conversation.Message;

namespace Application.UseCases.Conversation;

public class UseCaseFetchMessageForAConversation : IUseCaseParameterizedQuery<IEnumerable<DtoOutputMessage>,
    DtoInputFetchMessagesForAConversation>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IMessageRepository _messageRepository;

    public UseCaseFetchMessageForAConversation(IConversationRepository conversationRepository,
        IMessageRepository messageRepository)
    {
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
    }

    public IEnumerable<DtoOutputMessage> Execute(DtoInputFetchMessagesForAConversation dto)
    {
        var dbConversation = _conversationRepository.FetchById(dto.conversationId);

        //Check if the user belong to the conversation
        if (dto.UserId != dbConversation.IdUser1 && dto.UserId != dbConversation.IdUser2)
            throw new Exception("Vous ne faites pas partie de la conversation");

        return Mapper.GetInstance()
            .Map<IEnumerable<DtoOutputMessage>>(_messageRepository.FetchByConversationid(dto.conversationId));
    }
}